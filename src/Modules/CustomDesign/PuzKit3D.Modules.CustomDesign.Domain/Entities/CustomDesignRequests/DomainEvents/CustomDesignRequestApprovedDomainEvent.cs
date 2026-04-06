using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests.DomainEvents;

public record CustomDesignRequestApprovedDomainEvent(
    Guid CustomDesignRequestId,
    Guid CustomerId,
    CustomDesignRequestType Type,
    string? Sketches,
    string? CustomerPrompt) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}