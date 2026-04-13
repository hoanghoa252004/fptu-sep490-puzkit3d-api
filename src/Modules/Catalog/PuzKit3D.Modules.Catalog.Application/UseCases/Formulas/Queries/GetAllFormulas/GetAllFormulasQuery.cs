using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetAllFormulas;

public sealed record GetAllFormulasQuery : IQuery<List<GetFormulaDetailedResponseDto>>;


