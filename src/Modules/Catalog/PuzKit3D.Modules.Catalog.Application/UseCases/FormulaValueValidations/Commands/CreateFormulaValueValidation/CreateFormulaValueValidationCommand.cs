using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.CreateFormulaValueValidation;

public sealed record CreateFormulaValueValidationCommand(
    Guid FormulaId,
    decimal MinValue,
    decimal MaxValue,
    string Output) : ICommandT<Guid>;
