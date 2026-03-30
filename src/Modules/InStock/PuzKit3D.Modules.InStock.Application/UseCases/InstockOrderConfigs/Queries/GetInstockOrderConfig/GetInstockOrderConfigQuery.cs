using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Queries.GetInstockOrderConfig;

public record GetInstockOrderConfigQuery : IQuery<GetInstockOrderConfigResponse>;

public record GetInstockOrderConfigResponse(
    Guid Id,
    int OrderMustCompleteInDays,
    DateTime UpdatedAt);
