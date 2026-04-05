using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetRequestById;

public sealed record GetCustomDesignRequestByIdQuery(Guid Id) : IQuery<GetCustomDesignRequestByIdResponseDto>;

