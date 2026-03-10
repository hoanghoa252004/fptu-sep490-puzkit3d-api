using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailById;

internal sealed class GetInstockProductPriceDetailByIdQueryHandler 
    : IQueryHandler<GetInstockProductPriceDetailByIdQuery, GetInstockProductPriceDetailByIdResponseDto>
{
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;

    public GetInstockProductPriceDetailByIdQueryHandler(IInstockProductPriceDetailRepository priceDetailRepository)
    {
        _priceDetailRepository = priceDetailRepository;
    }

    public async Task<ResultT<GetInstockProductPriceDetailByIdResponseDto>> Handle(
        GetInstockProductPriceDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        var priceDetailId = InstockProductPriceDetailId.From(request.PriceDetailId);
        var priceDetail = await _priceDetailRepository.GetByIdAsync(priceDetailId, cancellationToken);

        if (priceDetail is null)
        {
            return Result.Failure<GetInstockProductPriceDetailByIdResponseDto>(
                InstockProductPriceDetailError.NotFound(request.PriceDetailId));
        }

        var response = new GetInstockProductPriceDetailByIdResponseDto(
            priceDetail.Id.Value,
            priceDetail.InstockPriceId.Value,
            priceDetail.InstockProductVariantId.Value,
            priceDetail.UnitPrice.Amount,
            priceDetail.IsActive,
            priceDetail.CreatedAt,
            priceDetail.UpdatedAt);

        return Result.Success(response);
    }
}
