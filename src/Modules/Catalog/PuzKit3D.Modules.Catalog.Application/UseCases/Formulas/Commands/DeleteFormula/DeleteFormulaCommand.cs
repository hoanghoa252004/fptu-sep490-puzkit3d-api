using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.DeleteFormula;

public sealed record DeleteFormulaCommand(Guid Id) : ICommand;
