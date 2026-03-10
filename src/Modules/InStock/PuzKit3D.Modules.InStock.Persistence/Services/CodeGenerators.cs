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
