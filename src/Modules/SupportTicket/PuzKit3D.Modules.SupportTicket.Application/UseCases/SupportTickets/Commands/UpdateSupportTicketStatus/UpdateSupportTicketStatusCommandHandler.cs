using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.UpdateSupportTicketStatus;

internal sealed class UpdateSupportTicketStatusCommandHandler
    : ICommandHandler<UpdateSupportTicketStatusCommand>
{
    private readonly ISupportTicketRepository _repository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;

    public UpdateSupportTicketStatusCommandHandler(
        ISupportTicketRepository repository,
        ISupportTicketUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateSupportTicketStatusCommand request,
        CancellationToken cancellationToken)
    {
        var ticketResult = await _repository.GetByIdAsync(
            SupportTicketId.From(request.SupportTicketId),
            cancellationToken);

        if (ticketResult.IsFailure)
            return Result.Failure(ticketResult.Error);

        var ticket = ticketResult.Value;
        ticket.UpdateStatus(request.NewStatus);

        await _repository.UpdateAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
