using MediatR;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Queries.GetInstockOrderConfig;

internal sealed class GetInstockOrderConfigQueryHandler : IQueryHandler<GetInstockOrderConfigQuery, GetInstockOrderConfigResponse>
{
    private readonly IInstockOrderConfigRepository _instockOrderConfigRepository;

    public GetInstockOrderConfigQueryHandler(IInstockOrderConfigRepository instockOrderConfigRepository)
    {
        _instockOrderConfigRepository = instockOrderConfigRepository;
    }

    public async Task<ResultT<GetInstockOrderConfigResponse>> Handle(
        GetInstockOrderConfigQuery request,
        CancellationToken cancellationToken)
    {
        var instockOrderConfig = await _instockOrderConfigRepository.GetFirstAsync(cancellationToken);

        if (instockOrderConfig is null)
        {
            return Result.Failure<GetInstockOrderConfigResponse>(
                InstockOrderError.InstockOrderConfigNotFound());
        }

        var response = new GetInstockOrderConfigResponse(
            instockOrderConfig.Id,
            instockOrderConfig.OrderMustCompleteInDays,
            instockOrderConfig.UpdatedAt);

        return Result.Success(response);
    }
}
