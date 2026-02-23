using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Data;

/// <summary>
/// Factory for creating database connections (for Dapper queries)
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new database connection
    /// </summary>
    IDbConnection CreateConnection();

    /// <summary>
    /// Creates a new database connection for a specific module
    /// </summary>
    IDbConnection CreateConnection(string connectionStringName);
}
