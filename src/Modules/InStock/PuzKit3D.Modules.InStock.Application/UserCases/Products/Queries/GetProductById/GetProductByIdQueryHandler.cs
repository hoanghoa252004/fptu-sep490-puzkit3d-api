using MediatR;
using PuzKit3D.SharedKernel.Application.Exceptions;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResponseDto>
{
    public async Task<ResultT<GetProductByIdResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
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
            throw new PuzKit3DException($"{nameof(GetProductByIdQuery)} Test PuzKit3DException"); 
        }
        else if (request.Id == 3)
        {
            throw new Exception("System Exception ( Test Wrap Exception )");
        }

        return Result.Success(new GetProductByIdResponseDto(
            Id: 1,
            Name: "3D Model Kit",
            Description: "Description for 3D Model Kit"
        ));
    }
}
