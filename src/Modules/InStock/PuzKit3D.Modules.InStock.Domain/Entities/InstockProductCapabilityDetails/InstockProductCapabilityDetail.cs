using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;

public sealed class InstockProductCapabilityDetail : Entity<Guid>
{
    public InstockProductId InstockProductId { get; private set; } = null!;
    public Guid CapabilityId { get; private set; }

    private InstockProductCapabilityDetail(
        Guid id,
        InstockProductId instockProductId,
        Guid capabilityId) : base(id)
    {
        InstockProductId = instockProductId;
        CapabilityId = capabilityId;
    }

    private InstockProductCapabilityDetail() : base()
    {
    }

    public static InstockProductCapabilityDetail Create(
        InstockProductId instockProductId,
        Guid capabilityId)
    {
        return new InstockProductCapabilityDetail(
            Guid.NewGuid(),
            instockProductId,
            capabilityId);
    }
}
