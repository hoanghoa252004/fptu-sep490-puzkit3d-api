using Microsoft.Extensions.Configuration;
using Npgsql;
using PuzKit3D.SharedKernel.Application.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Data;

/// <summary>
/// Factory for creating PostgreSQL database connections
/// </summary>
public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _defaultConnectionStringName;

    public DbConnectionFactory(
        IConfiguration configuration,
        string defaultConnectionStringName = "DefaultConnection")
    {
        _configuration = configuration;
        _defaultConnectionStringName = defaultConnectionStringName;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString(_defaultConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{_defaultConnectionStringName}' not found");

        return new NpgsqlConnection(connectionString);
    }

    public IDbConnection CreateConnection(string connectionStringName)
    {
        var connectionString = _configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found");

        return new NpgsqlConnection(connectionString);
    }
}
