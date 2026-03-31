using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetPartnerProductRequestById;

internal sealed class GetPartnerProductRequestByIdQueryHandler
    : IQueryHandler<GetPartnerProductRequestByIdQuery, object>
{
    private readonly IPartnerProductRequestRepository _repository;

    public GetPartnerProductRequestByIdQueryHandler(
        IPartnerProductRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var req = await _repository.GetByIdAsync(
            PartnerProductRequestId.From(request.Id),
            cancellationToken);

        if (req is null)
        {
            return Result.Failure<object>(PartnerProductRequestError.NotFound(request.Id));
        }

        var dto = new GetPartnerProductRequestByIdResponseDto(
            req.Id.Value,
            req.Code,
            req.CustomerId,
            req.PartnerId.Value,
            req.DesiredDeliveryDate,
            req.TotalRequestedQuantity,
            req.Note,
            req.Status,
            req.CreatedAt,
            req.UpdatedAt);

        return Result.Success((object)dto);
    }
}
