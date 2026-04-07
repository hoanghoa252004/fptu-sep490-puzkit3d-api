using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

public sealed class ProposalId : StronglyTypedId<Guid>
{
    private ProposalId(Guid value) : base(value) { }

    public static ProposalId Create() => new(Guid.NewGuid());

    public static ProposalId From(Guid value) => new(value);
}
