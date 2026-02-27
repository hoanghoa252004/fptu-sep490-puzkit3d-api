using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.UpdateAssemblyMethod;

public sealed record UpdateAssemblyMethodCommand(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive) : ICommand;


