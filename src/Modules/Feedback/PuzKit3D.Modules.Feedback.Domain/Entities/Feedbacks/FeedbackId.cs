using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;

public sealed class FeedbackId : StronglyTypedId<Guid>
{
    private FeedbackId(Guid value) : base(value) { }

    public static FeedbackId Create() => new(Guid.NewGuid());

    public static FeedbackId From(Guid value) => new(value);
}
