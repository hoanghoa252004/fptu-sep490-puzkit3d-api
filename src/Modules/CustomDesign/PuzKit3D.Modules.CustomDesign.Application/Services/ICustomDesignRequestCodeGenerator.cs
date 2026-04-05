using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Application.Services;

public interface ICustomDesignRequestCodeGenerator
{
    Task<string> GenerateCodeAsync(CancellationToken cancellationToken = default);
}
