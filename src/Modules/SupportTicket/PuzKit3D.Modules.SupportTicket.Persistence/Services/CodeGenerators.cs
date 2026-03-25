using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Services;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Services;

internal sealed class SupportTicketCodeGenerator : ISupportTicketCodeGenerator
{
    private readonly SupportTicketDbContext _context;

    public SupportTicketCodeGenerator(SupportTicketDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.SupportTickets
            .Where(p => p.Code.StartsWith("STI") && p.Code.Length == 6)
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);
        
        if (!allCodes.Any())
        {
            return "STI001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"STI{nextNumber:D3}";
    }
}