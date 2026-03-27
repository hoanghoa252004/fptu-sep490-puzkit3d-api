using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.UpdateSupportTicketStatus;

internal sealed class UpdateSupportTicketStatusCommandHandler
    : ICommandHandler<UpdateSupportTicketStatusCommand>
{
    private readonly ISupportTicketRepository _repository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateSupportTicketStatusCommandHandler(
        ISupportTicketRepository repository,
        ISupportTicketUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        UpdateSupportTicketStatusCommand request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var ticketResult = await _repository.GetByIdAsync(
            SupportTicketId.From(request.SupportTicketId),
            cancellationToken);

            if (ticketResult.IsFailure)
                return Result.Failure(ticketResult.Error);

            var ticket = ticketResult.Value;

            // Check if new status is same as current status
            if (ticket.Status == request.NewStatus)
                return Result.Failure(SupportTicketError.StatusAlreadySet(request.NewStatus));

            var isStaff = _currentUser.IsInRole(Roles.Staff);

            // Check authorization: Customer can only update to Resolved
            if (!isStaff && request.NewStatus != SupportTicketStatus.Resolved)
                return Result.Failure(SupportTicketError.UnauthorizedStatusTransition());

            // Validate status transition
            var updateResult = ticket.UpdateStatus(request.NewStatus);
            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            await _repository.UpdateAsync(ticket, cancellationToken);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        });
    }
}
