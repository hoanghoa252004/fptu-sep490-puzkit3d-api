using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CreateFormula;

public sealed record CreateFormulaCommand(
    string Code,
    string Expression,
    string? Description) : ICommandT<Guid>;
