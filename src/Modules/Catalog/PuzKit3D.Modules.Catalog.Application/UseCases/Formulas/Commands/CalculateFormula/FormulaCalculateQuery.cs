using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CalculateFormula;

public sealed record FormulaCalculateQuery(
    string FormulaCode,
    FormulaCalculateRequest Request) : IQuery<FormulaCalculateResponse>;

