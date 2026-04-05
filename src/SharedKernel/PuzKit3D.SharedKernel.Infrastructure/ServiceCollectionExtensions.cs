using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.SharedKernel.Application._3DModel;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Clock;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Identity;
using PuzKit3D.SharedKernel.Application.Image;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Queue;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Infrastructure._3DModel;
using PuzKit3D.SharedKernel.Infrastructure.Authentication;
using PuzKit3D.SharedKernel.Infrastructure.Clock;
using PuzKit3D.SharedKernel.Infrastructure.Data;
using PuzKit3D.SharedKernel.Infrastructure.Event;
using PuzKit3D.SharedKernel.Infrastructure.Identity;
using PuzKit3D.SharedKernel.Infrastructure.Image;
using PuzKit3D.SharedKernel.Infrastructure.Media;
using PuzKit3D.SharedKernel.Infrastructure.Queue;
using PuzKit3D.SharedKernel.Infrastructure.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure;

/// <summary>
/// Extension methods for registering SharedKernel Infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all SharedKernel Infrastructure services
    /// </summary>
    public static IServiceCollection AddSharedKernelInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<S3Settings>(configuration.GetSection(S3Settings.ConfigurationSection));

        // Register Services
        services.AddScoped<IMediaAssetService, MediaAssetService>();

        // CORS Configuration
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Core services
        services.AddHttpContextAccessor();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        // Identity, Authentication & Authorization
        services.AddIdentityInfrastructure(configuration);
        services.AddAuthenticationInfrastructure(configuration);
        services.AddAuthorizationInfrastructure();

        // Application services
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IIdentityService, IdentityService>();

        // Integration Events (In-Memory)
        services.AddScoped<IEventBus, InMemoryEventBus>();

        services.AddSingleton<IJobQueue, JobQueue>();
        services.AddScoped<IImageGenerationService, ImageGenerationService>();
        services.AddScoped<I3DModelGenerationService, _3DModelGenerationService>();

        return services;
    }

    /// <summary>
    /// Configures ASP.NET Core Identity with PostgreSQL
    /// </summary>
    private static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register IdentityDbContext with PostgreSQL
        services.AddDbContext<IdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Identity);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            })
            .UseSnakeCaseNamingConvention();

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        // Configure Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;

            // User settings
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IIdentityUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());
        return services;
    }

    /// <summary>
    /// Configures JWT Bearer authentication
    /// </summary>
    private static IServiceCollection AddAuthenticationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind JWT options from configuration
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        // Add authentication with JWT Bearer as default scheme
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(); // Register JWT Bearer handler

        // Configure JWT Bearer options from appsettings.json
        services.ConfigureOptions<JwtBearerConfigureOptions>();

        return services;
    }

    /// <summary>
    /// Configures role-based authorization
    /// </summary>
    private static IServiceCollection AddAuthorizationInfrastructure(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}