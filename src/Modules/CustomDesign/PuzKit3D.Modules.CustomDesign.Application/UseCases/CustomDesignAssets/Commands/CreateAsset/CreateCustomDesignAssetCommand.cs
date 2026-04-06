using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.CreateAsset;

public sealed record CreateCustomDesignAssetCommand(
    Guid RequestId,
    string CustomerPrompt) : ICommandT<Guid>;

