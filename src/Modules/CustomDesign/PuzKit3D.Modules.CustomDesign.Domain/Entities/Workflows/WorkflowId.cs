using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

public sealed class WorkflowId : StronglyTypedId<Guid>
{
    private WorkflowId(Guid value) : base(value) { }

    public static WorkflowId Create() => new(Guid.NewGuid());

    public static WorkflowId From(Guid value) => new(value);
}
