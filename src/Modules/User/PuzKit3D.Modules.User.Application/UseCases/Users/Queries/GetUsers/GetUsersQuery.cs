using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null) : IQuery<object>;
