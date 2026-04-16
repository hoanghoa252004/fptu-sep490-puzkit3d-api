using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.UpdateFormulaValueValidation;

public sealed record UpdateFormulaValueValidationCommand(
    Guid Id,
    decimal? MinValue = null,
    decimal? MaxValue = null,
    string? Output = null) : ICommandT<Guid>;
