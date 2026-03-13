namespace PuzKit3D.Modules.Payment.Api.Payments.CreatePaymentUrl;

public sealed record CreatePaymentUrlRequest(Guid OrderId, string provider);
