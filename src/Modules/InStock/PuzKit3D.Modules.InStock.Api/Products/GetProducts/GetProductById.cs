using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Api.Products.GetProducts;

public sealed class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/{id:int}", async (int id, ISender sender) =>
            {
                ResultT<GetProductByIdResponseDto> result = await sender.Send(new GetProductByIdQuery(id));

                return result!.MatchOk();
            })
            .WithName("GetProductById")
            .WithSummary("Get a product by Id.")
            .WithDescription("Get a product by Id with the specified details.")

            .AllowAnonymous()

            .Produces<GetProductByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
