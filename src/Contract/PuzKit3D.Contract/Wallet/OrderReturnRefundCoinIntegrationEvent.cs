using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Wallet;

public sealed record OrderReturnRefundCoinIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    decimal GrandTotalAmount) : IIntegrationEvent;
