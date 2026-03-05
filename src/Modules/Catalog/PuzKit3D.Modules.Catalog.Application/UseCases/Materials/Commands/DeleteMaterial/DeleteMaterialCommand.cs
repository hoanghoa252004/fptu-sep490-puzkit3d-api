using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.DeleteMaterial;

public sealed record DeleteMaterialCommand(Guid Id) : ICommand;
