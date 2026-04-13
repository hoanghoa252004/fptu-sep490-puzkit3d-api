using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetAllInstockProducts;

public sealed record GetAllInstockProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? DifficultyLevel = null,
    string? MaterialSlug = null,
    string? TopicSlug = null,
    List<string>? AssemblyMethodSlug = null,
    List<string>? CapabilitySlugs = null) : IQuery<PagedResult<object>>;
