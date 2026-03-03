using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCart;

public sealed record GetPartnerCartQuery(Guid UserId) : IQuery<CartDto>;
