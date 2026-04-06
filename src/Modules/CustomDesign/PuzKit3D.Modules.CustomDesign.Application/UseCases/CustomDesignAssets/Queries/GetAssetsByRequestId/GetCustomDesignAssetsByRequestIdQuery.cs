using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetsByRequestId;

public sealed record GetCustomDesignAssetsByRequestIdQuery(Guid RequestId) : IQuery<List<GetCustomDesignAssetsByRequestIdResponseDto>>;

public sealed record GetCustomDesignAssetsByRequestIdResponseDto(
    Guid Id,
    string Code,
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
