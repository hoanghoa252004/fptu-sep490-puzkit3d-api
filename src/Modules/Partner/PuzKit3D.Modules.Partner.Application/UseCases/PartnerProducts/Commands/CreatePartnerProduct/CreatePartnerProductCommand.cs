using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.CreatePartnerProduct;

public sealed record CreatePartnerProductCommand(
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    int Quantity,
    string ThumbnailUrl,
    Dictionary<string, string> PreviewAsset,
    string Slug,
    string? Description,
    bool IsActive) : ICommandT<Guid>;
