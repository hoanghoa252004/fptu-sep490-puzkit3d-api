using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetFormulaById;

public sealed record GetFormulaByIdQuery(Guid Id) : IQuery<GetFormulaDetailedResponseDto>;
