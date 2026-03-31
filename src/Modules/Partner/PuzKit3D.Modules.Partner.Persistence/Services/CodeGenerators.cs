using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Services;

namespace PuzKit3D.Modules.Partner.Persistence.Services;

internal sealed class PartnerProductRequestCodeGenerator : IPartnerProductRequestCodeGenerator
{
    private readonly PartnerDbContext _context;

    public PartnerProductRequestCodeGenerator(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.PartnerProductRequests
            .Where(r => r.Code.StartsWith("PPR") && r.Code.Length == 7)
            .Select(r => r.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "PPR0001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PPR{nextNumber:D4}";
    }
}

internal sealed class PartnerProductQuotationCodeGenerator : IPartnerProductQuotationCodeGenerator
{
    private readonly PartnerDbContext _context;

    public PartnerProductQuotationCodeGenerator(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.PartnerProductQuotations
            .Where(r => r.Code.StartsWith("PPQ") && r.Code.Length == 7)
            .Select(r => r.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "PPQ0001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PPQ{nextNumber:D4}";
    }
}

internal sealed class PartnerProductOrderCodeGenerator : IPartnerProductOrderCodeGenerator
{
    private readonly PartnerDbContext _context;

    public PartnerProductOrderCodeGenerator(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.PartnerProductOrders
            .Where(r => r.Code.StartsWith("PPO") && r.Code.Length == 7)
            .Select(r => r.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "PPO0001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"PPO{nextNumber:D4}";
    }
}
