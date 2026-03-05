using PuzKit3D.SharedKernel.Application.Authentication.Dtos;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Queries.GetProfile;

public sealed record GetProfileQuery(string UserId) : IQuery<UserDetailDto>;
