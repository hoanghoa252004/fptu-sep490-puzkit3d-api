using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.CreateInstockProduct;

public sealed record CreateInstockProductCommand(
    string Slug,
    string Name,
    int TotalPieceCount,
    string DifficultLevel,
    int EstimatedBuildTime,
    string ThumbnailUrl,
    Dictionary<string, string> PreviewAsset,
    Guid TopicId,
    Guid AssemblyMethodId,
    Guid CapabilityId,
    Guid MaterialId,
    string? Description = null,
    bool IsActive = false) : ICommandT<Guid>;

