using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetById;

public sealed record GetCustomDesignAssetByIdQuery(Guid Id) : IQuery<GetCustomDesignAssetByIdResponseDto>;

public sealed record GetCustomDesignAssetByIdResponseDto(
    Guid Id,
    string Code,
    Guid CustomDesignRequestId,
    int Version,
    string Status,
    List<string>? MultiviewImages,
    string? CompositeMultiviewImage,
    string? Rough3DModel,
    string? Rough3DModelTaskId,
    string? CustomerPrompt,
    string? NormalizePrompt,
    bool IsNeedSupport,
    bool IsFinalDesign,
    DateTime CreatedAt,
    DateTime UpdatedAt);
