using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.User;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.DeleteSupportTicket;

internal sealed class DeleteSupportTicketCommandHandler
    : IRequestHandler<DeleteSupportTicketCommand, DeleteSupportTicketResponse>
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

    public async Task<DeleteSupportTicketResponse> Handle(
        DeleteSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        // Get the support ticket
        var ticketResult = await _repository.GetByIdAsync(
            SupportTicketId.From(request.SupportTicketId),
            cancellationToken);

        if (ticketResult.IsFailure)
            throw new InvalidOperationException(ticketResult.Error.Message);

        var ticket = ticketResult.Value;

        // Check if status is Open
        if (ticket.Status != SupportTicketStatus.Open)
            throw new InvalidOperationException("Can only delete support tickets with Open status");

        // Check if user is the owner or staff
        var userId = Guid.Parse(_currentUser.UserId!);
        var isStaff = _currentUser.IsInRole("Staff") || _currentUser.IsInRole("System Administrator");

        if (!isStaff && ticket.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own support tickets");

        // Delete the support ticket
        var deleteResult = await _repository.DeleteAsync(ticket.Id, cancellationToken);
        if (deleteResult.IsFailure)
            throw new InvalidOperationException(deleteResult.Error.Message);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteSupportTicketResponse(
            true,
            "Support ticket deleted successfully");
    }
}
