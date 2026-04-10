using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetAllMaterials;

public sealed record GetAllMaterialsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    bool Ascending = true) : IQuery<PagedResult<object>>;
