using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResponseDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResultT<GetProductByIdResponseDto>> Handle(
        GetProductByIdQuery request, 
        CancellationToken cancellationToken)
    {
        // Get product by ID
        var productId = ProductId.From(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetProductByIdResponseDto>(
                ProductError.NotFound(request.Id));
        }

        // Map to DTO
        var response = new GetProductByIdResponseDto(
            Id: product.Id.Value,
            Name: product.Name,
            Price: product.Price,
            Stock: product.Stock);

        return Result.Success(response);
    }
}
