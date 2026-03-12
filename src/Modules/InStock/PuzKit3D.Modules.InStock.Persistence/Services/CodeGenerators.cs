using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Services;

namespace PuzKit3D.Modules.InStock.Persistence.Services;

internal sealed class InstockProductCodeGenerator : IInstockProductCodeGenerator
{
    private readonly InStockDbContext _context;

    public InstockProductCodeGenerator(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.InstockProducts
            .Where(p => p.Code.StartsWith("INP") && p.Code.Length == 6)
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);
        
        if (!allCodes.Any())
        {
            return "INP001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"INP{nextNumber:D3}";
    }
}

internal sealed class PartCodeGenerator : IPartCodeGenerator
{
    private readonly InStockDbContext _context;

    public PartCodeGenerator(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.InstockProducts
            .SelectMany(p => p.Parts)
            .Where(part => part.Code.StartsWith("PAR") && part.Code.Length == 7)
            .Select(part => part.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "PAR0001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PAR{nextNumber:D4}";
    }
}

internal sealed class PieceCodeGenerator : IPieceCodeGenerator
{
    private readonly InStockDbContext _context;

    public PieceCodeGenerator(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.InstockProducts
            .SelectMany(p => p.Parts)
            .SelectMany(part => part.Pieces)
            .Where(piece => piece.Code.StartsWith("PIE") && piece.Code.Length == 8)
            .Select(piece => piece.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "PIE00001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PIE{nextNumber:D5}";
    }
}

internal sealed class InstockProductVariantSkuGenerator : IInstockProductVariantSkuGenerator
{
    private readonly InStockDbContext _context;

    public InstockProductVariantSkuGenerator(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextSkuAsync(CancellationToken cancellationToken = default)
    {
        var allSkus = await _context.InstockProductVariants
            .Where(v => v.Sku.StartsWith("SKU") && v.Sku.Length == 6)
            .Select(v => v.Sku)
            .ToListAsync(cancellationToken);

        if (!allSkus.Any())
        {
            return "SKU001";
        }

        var maxSku = allSkus
            .Select(sku => int.TryParse(sku.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxSku + 1;
        return $"SKU{nextNumber:D3}";
    }
}

internal sealed class InstockOrderCodeGenerator : IInstockOrderCodeGenerator
{
    private readonly InStockDbContext _context;

    public InstockOrderCodeGenerator(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.InstockOrders
            .Where(o => o.Code.StartsWith("ORD") && o.Code.Length == 6)
            .Select(o => o.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "ORD001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"ORD{nextNumber:D3}";
    }
}


