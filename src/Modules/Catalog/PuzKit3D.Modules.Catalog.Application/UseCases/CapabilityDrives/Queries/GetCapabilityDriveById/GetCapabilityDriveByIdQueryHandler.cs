using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetCapabilityDriveById;

internal sealed class GetCapabilityDriveByIdQueryHandler : IQueryHandler<GetCapabilityDriveByIdQuery, GetCapabilityDriveByIdResponseDto>
{
    private readonly ICapabilityDriveRepository _capabilityDriveRepository;

    public GetCapabilityDriveByIdQueryHandler(ICapabilityDriveRepository capabilityDriveRepository)
    {
        _capabilityDriveRepository = capabilityDriveRepository;
    }

    public async Task<ResultT<GetCapabilityDriveByIdResponseDto>> Handle(GetCapabilityDriveByIdQuery request, CancellationToken cancellationToken)
    {
        var capabilityDriveId = CapabilityDriveId.From(request.Id);
        var capabilityDrive = await _capabilityDriveRepository.GetByIdAsync(capabilityDriveId, cancellationToken);

        if (capabilityDrive is null)
        {
            return Result.Failure<GetCapabilityDriveByIdResponseDto>(
                Error.NotFound("CapabilityDrive.NotFound", $"CapabilityDrive with ID {request.Id} not found"));
        }

        var response = new GetCapabilityDriveByIdResponseDto(
            capabilityDrive.Id.Value,
            capabilityDrive.CapabilityId.Value,
            capabilityDrive.DriveId.Value,
            capabilityDrive.Quantity);

        return Result.Success(response);
    }
}
