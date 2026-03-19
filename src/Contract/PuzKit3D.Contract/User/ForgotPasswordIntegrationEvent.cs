using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.User;

public sealed record ForgotPasswordIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    string Email,
    string UserId,
    string Token,
    string ResetUrl
) : IIntegrationEvent;
