namespace PuzKit3D.Modules.InStock.Application.Services;

public interface IInstockProductCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

public interface IInstockProductVariantSkuGenerator
{
    Task<string> GenerateNextSkuAsync(CancellationToken cancellationToken = default);
}

public interface IInstockOrderCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}


