using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

public sealed record GetAssemblyMethodByIdQuery(Guid Id) : IQuery<GetAssemblyMethodByIdResponseDto>;
