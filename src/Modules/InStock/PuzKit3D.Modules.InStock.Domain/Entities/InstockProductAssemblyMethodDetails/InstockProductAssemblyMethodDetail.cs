using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductAssemblyMethodDetails;

public sealed class InstockProductAssemblyMethodDetail : Entity<Guid>
{
    public InstockProductId InstockProductId { get; private set; } = null!;
    public Guid AssemblyMethodId { get; private set; }

    private InstockProductAssemblyMethodDetail(
        Guid id,
        InstockProductId instockProductId,
        Guid assemblyMethodId) : base(id)
    {
        InstockProductId = instockProductId;
        AssemblyMethodId = assemblyMethodId;
    }

    private InstockProductAssemblyMethodDetail() : base()
    {
    }

    public static InstockProductAssemblyMethodDetail Create(
        InstockProductId instockProductId,
        Guid assemblyMethodId)
    {
        return new InstockProductAssemblyMethodDetail(
            Guid.NewGuid(),
            instockProductId,
            assemblyMethodId);
    }
}
