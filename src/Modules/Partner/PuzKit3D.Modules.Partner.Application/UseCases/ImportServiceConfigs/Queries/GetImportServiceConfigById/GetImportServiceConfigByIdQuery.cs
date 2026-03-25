using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigById;

public sealed record GetImportServiceConfigByIdQuery(Guid Id) : IQuery<object>;
