using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetDriveById;

public sealed record GetDriveByIdQuery(Guid Id) : IQuery<object>;
