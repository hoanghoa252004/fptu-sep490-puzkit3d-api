using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.UpdatePartnerProduct;

public sealed record UpdatePartnerProductCommand(
    Guid Id,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    Dictionary<string, string> PreviewAsset,
    string Slug,
    string? Description = null) : ICommand;
