namespace PuzKit3D.Modules.Payment.Api.Transactions.CreateTransaction;

public sealed record CreateTransactionRequestDto(Guid paymentId, string provider);
