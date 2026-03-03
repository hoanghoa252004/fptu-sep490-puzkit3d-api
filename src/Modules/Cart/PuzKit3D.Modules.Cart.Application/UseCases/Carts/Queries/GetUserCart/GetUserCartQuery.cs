using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;

public sealed record GetUserCartQuery(
    Guid UserId,
    Guid CartTypeId) : IQuery<CartDto>;

