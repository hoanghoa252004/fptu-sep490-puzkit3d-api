using MediatR;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests.DomainEvents;
using PuzKit3D.SharedKernel.Application.Queue;

namespace PuzKit3D.Modules.CustomDesign.Application.DomainEventHandlers.CustomDesignRequests;

internal sealed class CustomDesignRequestApprovedDomainEventHandler
    : INotificationHandler<CustomDesignRequestApprovedDomainEvent>
{
    private readonly ICustomDesignAssetRepository _assetRepository;
    private readonly ICustomDesignAssetCodeGenerator _codeGenerator;

    public CustomDesignRequestApprovedDomainEventHandler(
        ICustomDesignAssetRepository assetRepository,
        ICustomDesignAssetCodeGenerator codeGenerator)
    {
        _assetRepository = assetRepository;
        _codeGenerator = codeGenerator;
    }

    public async Task Handle(
        CustomDesignRequestApprovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var assetCode = await _codeGenerator.GenerateCodeAsync(cancellationToken);

        // Create new CustomDesignAsset
        var asset = CustomDesignAsset.Create(
            id: Guid.NewGuid(),
            code: assetCode,
            customDesignRequestId: notification.CustomDesignRequestId,
            version: 0,
            multiviewImages: null,
            compositeMultiviewImage: null,
            rough3DModel: null,
            rough3DModelTaskId: null,
            customerPrompt: notification.CustomerPrompt,
            normalizePrompt: null,
            isNeedSupport: false,
            isFinalDesign: false,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow);

        // Add to repository
        await _assetRepository.AddAsync(asset, cancellationToken);
    }
}
