using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using PuzKit3D.Application.UserCases.Products.Queries.GetProductById;
using PuzKit3D.Contract.Abstractions.Shared.Results;
using PuzKit3D.Presentation.Abstractions;
using PuzKit3D.Presentation.Endpoints.Group;
using PuzKit3D.Presentation.Results.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Presentation.Endpoints.UseCases.Products.GetProducts;

internal sealed class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/{id:int}", async (int id, ISender sender) =>
            {
                Result<GetProductByIdResponseDto> result = await sender.Send(new GetProductByIdQuery(id));

                return result.MatchOk();
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
