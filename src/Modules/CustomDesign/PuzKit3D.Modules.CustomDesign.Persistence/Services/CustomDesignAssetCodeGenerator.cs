using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Services;

internal class CustomDesignAssetCodeGenerator : ICustomDesignAssetCodeGenerator
{
    private const string Prefix = "CDA";

    private readonly CustomDesignDbContext _context;

    public CustomDesignAssetCodeGenerator(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateCodeAsync(CancellationToken cancellationToken = default)
    {
        var allCodes = await _context.CustomDesignAssets
            .Where(p => p.Code.StartsWith(Prefix) && p.Code.Length == 6)
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);

        if (!allCodes.Any())
        {
            return $"{Prefix}001";
        }

        var maxCode = allCodes
            .Select(code => int.TryParse(code.Substring(3), out var num) ? num : 0)
            .Max();

        var nextNumber = maxCode + 1;
        return $"{Prefix}{nextNumber:D3}";
    }
}
