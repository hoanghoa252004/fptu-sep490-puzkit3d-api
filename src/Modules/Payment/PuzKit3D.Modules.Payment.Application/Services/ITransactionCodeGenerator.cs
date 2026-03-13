namespace PuzKit3D.Modules.Payment.Application.Services;

public interface ITransactionCodeGenerator
{
    /// <summary>
    /// Generates the next transaction code with format TRA00001, TRA00002, etc.
    /// </summary>
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}
