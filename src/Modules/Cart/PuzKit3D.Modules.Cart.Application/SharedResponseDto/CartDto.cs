using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Cart.Application.SharedResponseDto;

public sealed record CartDto(
    Guid Id,
    Guid UserId,
    string CartType,
    int TotalItem,
    List<CartItemDto> Items);
