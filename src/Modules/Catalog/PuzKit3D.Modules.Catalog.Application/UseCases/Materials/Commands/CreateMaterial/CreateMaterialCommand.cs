using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.CreateMaterial;

public sealed record CreateMaterialCommand(
    string Name,
    string Slug,
    string? Description,
    bool IsActive) : ICommandT<Guid>;
