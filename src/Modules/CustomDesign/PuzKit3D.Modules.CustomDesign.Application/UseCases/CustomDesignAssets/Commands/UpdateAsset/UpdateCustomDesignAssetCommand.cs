using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.UpdateAsset;

public sealed record UpdateCustomDesignAssetCommand(
    Guid Id,
    List<string>? MultiviewImages,
    bool? IsNeedSupport,
    bool? IsFinalDesign) : ICommand;
