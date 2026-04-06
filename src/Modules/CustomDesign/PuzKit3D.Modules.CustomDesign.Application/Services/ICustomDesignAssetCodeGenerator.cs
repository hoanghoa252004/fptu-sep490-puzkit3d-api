using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Application.Services;

public interface ICustomDesignAssetCodeGenerator
{
    Task<string> GenerateCodeAsync(CancellationToken cancellationToken = default);
}
