using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetPartnerProductRequestDetail;

internal sealed class GetPartnerProductRequestDetailQueryHandler
    : IQueryHandler<GetPartnerProductRequestDetailQuery, object>
{
    private readonly IPartnerProductRequestRepository _repository;
    private readonly IPartnerProductRequestDetailRepository _detailRepository;

    public GetPartnerProductRequestDetailQueryHandler(
        IPartnerProductRequestRepository repository,
        IPartnerProductRequestDetailRepository detailRepository)
    {
        _repository = repository;
        _detailRepository = detailRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductRequestDetailQuery request,
        CancellationToken cancellationToken)
    {
        var req = await _repository.GetByIdAsync(
            PartnerProductRequestId.From(request.Id),
            cancellationToken);

        if (req is null)
        {
            return Result.Failure<object>(PartnerProductRequestError.NotFound(request.Id));
        }

        var details = await _detailRepository.FindByRequestIdAsync(
            PartnerProductRequestId.From(request.Id),
            cancellationToken);

        var detailDtos = details
            .Select(d => new RequestDetailItemDto(
                d.Id.Value,
                d.PartnerProductId.Value,
                d.Quantity))
            .ToList();

        var dto = new GetPartnerProductRequestDetailResponseDto(
            req.Id.Value,
            req.Code,
            req.CustomerId,
            req.PartnerId.Value,
            req.DesiredDeliveryDate,
            req.TotalRequestedQuantity,
            req.Note,
            req.Status,
            req.CreatedAt,
            req.UpdatedAt,
            detailDtos);

        return Result.Success((object)dto);
    }
}
