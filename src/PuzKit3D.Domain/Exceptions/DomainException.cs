using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Domain.Exceptions;

/// <summary>
/// Base exception for all DOMAIN-RELATED error
/// Bắt mấy cái business rule violations
/// </summary>
public  class DomainException : Exception
{
    
    protected DomainException(string message) 
        : base(message) { }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
