using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetRequirementById;

public sealed record GetCustomDesignRequirementByIdQuery(Guid Id) : IQuery<GetCustomDesignRequirementByIdResponseDto>;
