using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.CreateSupportTicket;

internal sealed class CreateSupportTicketCommandHandler
    : ICommandTHandler<CreateSupportTicketCommand, Guid>
{
    private readonly ISupportTicketRepository _repository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;

    public CreateSupportTicketCommandHandler(
        ISupportTicketRepository repository,
        ISupportTicketUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
    {
        var result = SupportTicketEntity.Create(
            request.UserId,
            request.OrderId,
            request.Type,
            request.Reason,
            request.Proof);

        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        var ticket = result.Value;

        await _repository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
