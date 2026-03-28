using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Cart.Application.SharedResponseDto;

public sealed record CartItemDto(
Guid Id,
Guid ItemId,
decimal? UnitPrice,
Guid? InStockProductPriceDetailId,
int Quantity,
decimal? TotalPrice,
ProductDetailsDto? ProductDetails,
bool IsVariantActive = true,
bool IsValidPrice = true,
decimal? NewUnitPrice = null,
Guid? NewPriceDetailId = null,
string? NewPriceName = null,
bool IsValidInventory = true,
int? AvailableInventory = null);