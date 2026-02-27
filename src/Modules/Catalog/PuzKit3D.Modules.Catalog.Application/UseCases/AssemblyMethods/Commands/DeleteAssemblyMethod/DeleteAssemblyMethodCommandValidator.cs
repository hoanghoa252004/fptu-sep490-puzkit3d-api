using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.DeleteAssemblyMethod;

internal sealed class DeleteAssemblyMethodCommandValidator : AbstractValidator<DeleteAssemblyMethodCommand>
{
    public DeleteAssemblyMethodCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Assembly method ID is required");
    }
}
