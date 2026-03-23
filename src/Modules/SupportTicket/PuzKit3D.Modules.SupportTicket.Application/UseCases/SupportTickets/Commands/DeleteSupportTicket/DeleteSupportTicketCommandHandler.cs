using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.DeleteSupportTicket;

internal sealed class DeleteSupportTicketCommandHandler
    : ICommandHandler<DeleteSupportTicketCommand>
{
    private readonly ISupportTicketRepository _repository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteSupportTicketCommandHandler(
        ISupportTicketRepository repository,
        ISupportTicketUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        DeleteSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        // Get the support ticket
        var ticketResult = await _repository.GetByIdAsync(
            SupportTicketId.From(request.SupportTicketId),
            cancellationToken);

        if (ticketResult.IsFailure)
            return Result.Failure(ticketResult.Error);

        var ticket = ticketResult.Value;

        // Check if status is Open
        if (ticket.Status != SupportTicketStatus.Open)
            return Result.Failure(SupportTicketError.CanOnlyDeleteOpenTickets());

        // Check if user is the owner or staff
        var userId = Guid.Parse(_currentUser.UserId!);
        var isStaff = _currentUser.IsInRole(Roles.Staff);

        if (!isStaff && ticket.UserId != userId)
            return Result.Failure(SupportTicketError.Unauthorized());

        // Emit delete event before deleting
        ticket.Delete();

        // Delete the support ticket
        var deleteResult = await _repository.DeleteAsync(ticket.Id, cancellationToken);
        if (deleteResult.IsFailure)
            return Result.Failure(deleteResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
