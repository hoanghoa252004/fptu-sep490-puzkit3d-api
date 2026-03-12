using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Cart.Application.SharedResponseDto;

public sealed record ProductDetailsDto(
    string Name,
    string? Sku,
    string? Color,
    int? AssembledLengthMm,
    int? AssembledWidthMm,
    int? AssembledHeightMm,
    string? ThumbnailUrl,
    bool IsActive);
