using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetPartnerProductOrderById;

internal sealed class GetPartnerProductOrderByIdQueryHandler
    : IQueryHandler<GetPartnerProductOrderByIdQuery, object>
{
    private readonly IPartnerProductOrderRepository _repository;

    public GetPartnerProductOrderByIdQueryHandler(IPartnerProductOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdWithDetailsAsync(
            PartnerProductOrderId.From(request.Id),
            cancellationToken);

        if (order is null)
        {
            return Result.Failure<object>(PartnerProductOrderError.NotFound(request.Id));
        }

        var dto = new GetPartnerProductOrderByIdResponseDto(
            order.Id.Value,
            order.Code,
            order.CustomerId,
            order.CustomerName,
            order.CustomerPhone,
            order.CustomerEmail,
            order.CustomerProvinceName,
            order.CustomerDistrictName,
            order.CustomerWardName,
            order.SubTotalAmount,
            order.ShippingFee,
            order.ImportTaxAmount,
            order.GrandTotalAmount,
            order.Status.ToString(),
            order.PaymentMethod,
            order.IsPaid,
            order.CreatedAt,
            order.UpdatedAt,
            order.PaidAt);

        return Result.Success<object>(dto);
    }
}
