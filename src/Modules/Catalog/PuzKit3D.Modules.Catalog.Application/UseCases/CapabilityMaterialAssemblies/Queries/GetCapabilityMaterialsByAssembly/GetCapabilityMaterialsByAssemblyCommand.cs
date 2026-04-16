using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetCapabilityMaterialAssembliesByAssemblyMethodId;

public sealed record GetCapabilityMaterialAssembliesByAssemblyMethodIdQuery(Guid AssemblyMethodId) : IQuery<object>;
