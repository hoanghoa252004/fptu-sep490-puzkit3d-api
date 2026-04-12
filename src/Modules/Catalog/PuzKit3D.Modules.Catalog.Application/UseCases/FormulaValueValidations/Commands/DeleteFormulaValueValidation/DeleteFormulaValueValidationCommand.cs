using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.DeleteFormulaValueValidation;

public sealed record DeleteFormulaValueValidationCommand(Guid Id) : ICommand;
