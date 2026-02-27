using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.CreateAssemblyMethod;

public sealed record CreateAssemblyMethodCommand(
    string Name,
    string Slug,
    string? Description,
    bool IsActive) : ICommandT<Guid>;
