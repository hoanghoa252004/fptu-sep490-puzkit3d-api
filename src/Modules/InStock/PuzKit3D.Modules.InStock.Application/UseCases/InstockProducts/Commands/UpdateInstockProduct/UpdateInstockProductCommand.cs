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
Guid? MaterialId,
List<Guid>? CapabilityIds,
List<Guid>? AssemblyMethodIds = null,
List<UpdateDriveDetailDto>? Drives = null,
string? Description = null,
bool? IsActive = null) : ICommand;




