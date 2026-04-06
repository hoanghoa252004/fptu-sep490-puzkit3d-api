using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetAllRequirements;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequirements.GetAllRequirements;

internal sealed class GetAllCustomDesignRequirements : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequirementGroup()
            .MapGet("/", async (
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                // Check if customer: only active, else: all
                var isCustomer = !httpContext.User.IsInRole(Roles.Staff) && !httpContext.User.IsInRole(Roles.BusinessManager);
                var query = new GetAllCustomDesignRequirementsQuery(OnlyActive: isCustomer);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllCustomDesignRequirements")
            .WithSummary("Get all custom design requirements")
            .WithDescription("Retrieves all custom design requirements. Customers: only active requirements. Staff/Manager: all requirements.")
            .RequireAuthorization()
            .Produces<IEnumerable<GetAllCustomDesignRequirementsResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
