using MediatR;
using PuzKit3D.Application.Exceptions;
using PuzKit3D.Contract.Abstractions.Message;
using PuzKit3D.Contract.Abstractions.Shared.Errors;
using PuzKit3D.Contract.Abstractions.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Application.UserCases.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResponseDto>
{
    public async Task<Result<GetProductByIdResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == 1)
        {
            return Result.Failure<GetProductByIdResponseDto>(
                Error.NotFound(
                    "Product.NotFound",
                    $"Product with Id [{request.Id}] not found"
                )
            );
        }
        else if (request.Id == 2)
        {
            throw new PuzKit3DException(nameof(GetProductByIdQuery));
        }
        
        return Result.Success(new GetProductByIdResponseDto(
            Id: 1,
            Name: "3D Model Kit",
            Description: "Description for 3D Model Kit"
        ));
    }
}
