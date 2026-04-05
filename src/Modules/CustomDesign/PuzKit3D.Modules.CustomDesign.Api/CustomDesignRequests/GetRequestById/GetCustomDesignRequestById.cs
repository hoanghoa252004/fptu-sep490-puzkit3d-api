using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetRequestById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequests.GetRequestById;

internal sealed class GetCustomDesignRequestById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequestGroup()
            .MapGet("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCustomDesignRequestByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetCustomDesignRequestById")
            .WithSummary("Get a custom design request by ID")
            .WithDescription("Gets a specific custom design request by ID. Customers can only view their own requests. Staff and Business Manager can view all requests.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<GetCustomDesignRequestByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

