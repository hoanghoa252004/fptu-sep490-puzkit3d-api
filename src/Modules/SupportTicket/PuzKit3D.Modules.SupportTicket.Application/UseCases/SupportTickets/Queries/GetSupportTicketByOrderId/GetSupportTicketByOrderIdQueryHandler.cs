using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTicketByOrderId;

internal sealed class GetSupportTicketByOrderIdQueryHandler
    : IQueryHandler<GetSupportTicketByOrderIdQuery, SupportTicketDto>
{
    private readonly ISupportTicketRepository _repository;

    public GetSupportTicketByOrderIdQueryHandler(ISupportTicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<SupportTicketDto>> Handle(
        GetSupportTicketByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (result.IsFailure)
            return Result.Failure<SupportTicketDto>(result.Error);

        if (!result.Value.Any())
        {
            return Result.Failure<SupportTicketDto>(
                SupportTicketError.SupportTicketNotFound($"No support ticket found for order {request.OrderId}"));
        }

        var ticket = result.Value.First();

        var details = ticket.Details
            .Select(d => new SupportTicketDetailDto(
                d.Id.Value,
                d.OrderItemId,
                d.PartId,
                d.Quantity,
                d.Note))
            .ToList();

        var dto = new SupportTicketDto(
            ticket.Id.Value,
            ticket.Code,
            ticket.UserId,
            ticket.OrderId,
            ticket.Type.ToString(),
            ticket.Status.ToString(),
            ticket.Reason,
            ticket.Proof,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            details);
        return Result.Success(dto);
    }
}
