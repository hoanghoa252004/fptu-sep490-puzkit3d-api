using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.UpdateInstockProduct;

public sealed record UpdateInstockProductCommand(
Guid Id,
string? Slug,
string? Name,
int? TotalPieceCount,
string? DifficultLevel,
int? EstimatedBuildTime,
string? ThumbnailUrl,
Dictionary<string, string>? PreviewAsset,
Guid? TopicId,
Guid? AssemblyMethodId,
List<Guid>? CapabilityIds,
Guid? MaterialId,
string? Description,
bool? IsActive) : ICommand;
