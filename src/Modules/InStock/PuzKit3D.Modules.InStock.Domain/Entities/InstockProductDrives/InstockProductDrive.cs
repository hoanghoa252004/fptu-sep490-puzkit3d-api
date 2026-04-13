using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;

public sealed class InstockProductDrive : Entity<InstockProductDriveId>
{
    public InstockProductId InstockProductId { get; private set; } = null!;
    public Guid DriveId { get; private set; }
    public int Quantity { get; private set; }

    private InstockProductDrive(
        InstockProductDriveId id,
        InstockProductId instockProductId,
        Guid driveId,
        int quantity) : base(id)
    {
        InstockProductId = instockProductId;
        DriveId = driveId;
        Quantity = quantity;
    }

    private InstockProductDrive() : base()
    {
    }

    public static InstockProductDrive Create(
        InstockProductId instockProductId,
        Guid driveId,
        int quantity)
    {
        return new InstockProductDrive(
            InstockProductDriveId.Create(),
            instockProductId,
            driveId,
            quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
