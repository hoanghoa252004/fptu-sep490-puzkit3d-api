using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetAllRequirements;

public sealed record GetAllCustomDesignRequirementsQuery(bool OnlyActive = true) : IQuery<IEnumerable<GetAllCustomDesignRequirementsResponseDto>>;
