using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;

namespace PuzKit3D.Modules.InStock.Persistence.Services;

internal sealed class InstockProductCodeGenerator : IInstockProductCodeGenerator
{
    private readonly IInstockProductRepository _productRepository;

    public InstockProductCodeGenerator(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        
        if (!allProducts.Any())
        {
            return "INP001";
        }

        var maxCode = allProducts
            .Select(p => p.Code)
            .Where(code => code.StartsWith("INP") && code.Length == 6)
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .DefaultIfEmpty(0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"INP{nextNumber:D3}";
    }
}

internal sealed class PartCodeGenerator : IPartCodeGenerator
{
    private readonly IInstockProductRepository _productRepository;

    public PartCodeGenerator(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var allParts = allProducts.SelectMany(p => p.Parts).ToList();

        if (!allParts.Any())
        {
            return "PAR0001";
        }

        var maxCode = allParts
            .Select(p => p.Code)
            .Where(code => code.StartsWith("PAR") && code.Length == 7)
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .DefaultIfEmpty(0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PAR{nextNumber:D4}";
    }
}

internal sealed class PieceCodeGenerator : IPieceCodeGenerator
{
    private readonly IInstockProductRepository _productRepository;

    public PieceCodeGenerator(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var allPieces = allProducts
            .SelectMany(p => p.Parts)
            .SelectMany(part => part.Pieces)
            .ToList();

        if (!allPieces.Any())
        {
            return "PIE00001";
        }

        var maxCode = allPieces
            .Select(p => p.Code)
            .Where(code => code.StartsWith("PIE") && code.Length == 8)
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .DefaultIfEmpty(0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PIE{nextNumber:D5}";
    }
}
