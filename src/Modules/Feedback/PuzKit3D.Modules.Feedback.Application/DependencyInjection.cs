using Microsoft.Extensions.DependencyInjection;

namespace PuzKit3D.Modules.Feedback.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFeedbackApplication(this IServiceCollection services)
    {
        // Handlers and validators are registered via reflection by SharedKernelApplication
        return services;
    }
}
