using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.UpdateFormula;

public sealed record UpdateFormulaCommand(
    Guid Id,
    string? Expression = null,
    string? Description = null) : ICommandT<Guid>;






