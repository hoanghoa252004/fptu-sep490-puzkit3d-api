using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Authentication;

internal sealed class JwtBearerConfigureOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IConfiguration _configuration;

    public JwtBearerConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is System.Security.Claims.ClaimsIdentity claimsIdentity)
                {
                    // Ensure AuthenticationType is set - this is critical for IsAuthenticated to return true
                    if (string.IsNullOrEmpty(claimsIdentity.AuthenticationType))
                    {
                        // Create a new ClaimsIdentity with explicit AuthenticationType
                        var authenticatedIdentity = new System.Security.Claims.ClaimsIdentity(
                            claimsIdentity.Claims,
                            JwtBearerDefaults.AuthenticationScheme,
                            claimsIdentity.NameClaimType,
                            claimsIdentity.RoleClaimType);

                        context.Principal = new System.Security.Claims.ClaimsPrincipal(authenticatedIdentity);
                        claimsIdentity = authenticatedIdentity;
                    }

                    // Map 'sub' claim to NameIdentifier if not already present
                    var subClaim = claimsIdentity.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
                    if (subClaim != null && !claimsIdentity.HasClaim(System.Security.Claims.ClaimTypes.NameIdentifier, subClaim.Value))
                    {
                        claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, subClaim.Value));
                    }

                    // Map 'email' claim to Email if not already present
                    var emailClaim = claimsIdentity.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email);
                    if (emailClaim != null && !claimsIdentity.HasClaim(System.Security.Claims.ClaimTypes.Email, emailClaim.Value))
                    {
                        claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, emailClaim.Value));
                    }

                    // Map 'email' claim to Name (UserName) if not already present
                    if (emailClaim != null && !claimsIdentity.HasClaim(System.Security.Claims.ClaimTypes.Name, emailClaim.Value))
                    {
                        claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, emailClaim.Value));
                    }
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Log authentication failures for debugging
                var exception = context.Exception;
                // You can add logging here if needed
                return Task.CompletedTask;
            }
        };
    }
}