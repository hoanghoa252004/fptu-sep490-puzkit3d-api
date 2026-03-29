using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigsForSelect;

public sealed record GetImportServiceConfigsForSelectQuery : IQuery<IEnumerable<object>>;
