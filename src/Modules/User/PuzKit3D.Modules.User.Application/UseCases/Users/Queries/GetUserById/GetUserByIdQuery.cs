using PuzKit3D.SharedKernel.Application.Authentication.Dtos;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(string UserId) : IQuery<UserDetailDto>;
