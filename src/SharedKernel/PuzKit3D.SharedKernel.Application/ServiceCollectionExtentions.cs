using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.SharedKernel.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddSharedKernelApplication(this IServiceCollection services, Assembly[] moduleAssemblies)
    {
        // ====== MEDIATR ======
        services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk4MTU2ODAwIiwiaWF0IjoiMTc2NjYzNjI1NSIsImFjY291bnRfaWQiOiIwMTliNTNiOThhNmU3ZjA3OWU2MmVmOGRjYjQ1YmY0NCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2Q5dmt5Z3B3N2pobXpxbWh4MTJrOW45Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.FoCNMRbuNCEC7S2w5kjBvpe_-MgHeEjnsaD3zO4KQHxzmiSy-v5AJXileMgGq6Vb-BKM43Onlvy8nhIlddhNutmQhSsXggu6mhhCw4LedKL-0dOry9V0OAq98qQefYn47qRzbeS2gsz4gwvSMwpJ4NsE-YLDgkE8nJwrnigu3Wk4x30Hg6dv7F8lhD8zSlZXzSJACqo2QQKzVygP-ASutbRWnssoQAhStHr7u0_gajN7ktqpFppOO7zcCykUBcQ9DNRb-hYcI43A1UwiXhAsVeb6-x9Rr_fXzIQC5wlm7HrYWqt91C1WP2zmSO6ZDbwvrs-VYWv0eQ8NBVU5zC-B-w";

                cfg.RegisterServicesFromAssemblies(moduleAssemblies);

                cfg.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            }
        );

        // ===== FLUENT VALIDATION ======
        services.AddValidatorsFromAssemblies(moduleAssemblies,
            includeInternalTypes: true);

        return services;
    }
}
