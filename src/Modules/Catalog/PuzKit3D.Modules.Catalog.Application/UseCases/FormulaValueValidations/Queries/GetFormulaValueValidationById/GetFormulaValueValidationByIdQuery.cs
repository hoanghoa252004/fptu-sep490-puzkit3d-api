using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetFormulaValueValidationById;

public sealed record GetFormulaValueValidationByIdQuery(Guid Id) : IQuery<object>;
