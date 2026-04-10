using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.UpdateMaterial;

public sealed record UpdateMaterialCommand(
    Guid Id,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    bool IsActive) : ICommand;

