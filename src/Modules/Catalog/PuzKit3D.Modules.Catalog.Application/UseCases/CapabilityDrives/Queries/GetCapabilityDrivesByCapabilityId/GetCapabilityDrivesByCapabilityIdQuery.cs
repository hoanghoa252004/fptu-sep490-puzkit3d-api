using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetCapabilityDrivesByCapabilityId;

public sealed record GetCapabilityDrivesByCapabilityIdQuery(Guid CapabilityId) : IQuery<object>;
