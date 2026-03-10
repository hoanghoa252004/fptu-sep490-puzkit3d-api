using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetInstockPriceById;

internal sealed class GetInstockPriceByIdQueryHandler : IQueryHandler<GetInstockPriceByIdQuery, GetInstockPriceByIdResponseDto>
{
    private readonly IInstockPriceRepository _priceRepository;

    public GetInstockPriceByIdQueryHandler(IInstockPriceRepository priceRepository)
    {
        _priceRepository = priceRepository;
    }

    public async Task<ResultT<GetInstockPriceByIdResponseDto>> Handle(
        GetInstockPriceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var priceId = InstockPriceId.From(request.PriceId);
        var price = await _priceRepository.GetByIdAsync(priceId, cancellationToken);

        if (price is null)
        {
            return Result.Failure<GetInstockPriceByIdResponseDto>(
                InstockPriceError.NotFound(request.PriceId));
        }

        var response = new GetInstockPriceByIdResponseDto(
            price.Id.Value,
            price.Name,
            price.EffectiveFrom,
            price.EffectiveTo,
            price.Priority,
            price.IsActive,
            price.CreatedAt,
            price.UpdatedAt);

        return Result.Success(response);
    }
}
