namespace PuzKit3D.Modules.InStock.Application.Services;

public interface IInstockProductCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

public interface IPartCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

public interface IPieceCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}
