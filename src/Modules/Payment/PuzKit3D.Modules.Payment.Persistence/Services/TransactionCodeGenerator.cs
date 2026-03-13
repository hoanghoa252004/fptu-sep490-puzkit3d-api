using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.Services;

namespace PuzKit3D.Modules.Payment.Persistence.Services;

internal sealed class TransactionCodeGenerator : ITransactionCodeGenerator
{
    private readonly PaymentDbContext _context;

    public TransactionCodeGenerator(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.Transactions
            .Where(t => t.Code.StartsWith("TRA") && t.Code.Length == 8)
            .Select(t => t.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return "TRA00001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"TRA{nextNumber:D5}";
    }
}
