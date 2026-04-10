using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

public static class DriveError
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Drive.NotFound", $"Drive with ID {id} not found");
}
