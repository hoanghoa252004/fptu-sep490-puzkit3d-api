using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

public sealed class TopicId : StronglyTypedId<Guid>
{
    private TopicId(Guid value) : base(value) { }

    public static TopicId Create() => new(Guid.NewGuid());

    public static TopicId From(Guid value) => new(value);
}
