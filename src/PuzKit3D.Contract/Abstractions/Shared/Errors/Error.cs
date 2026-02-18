using PuzKit3D.Contract.Abstractions.Shared.Errors.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.Abstractions.Shared.Errors;

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    // Factory methods
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
            // 500 Internal Server Error

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);
            // 400 Bad Request  

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
            // 404 Not Found

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
            // 409 Conflict

    public static Error Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);
            // 401 Unauthorized

    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);
            // 403 Forbidden    
}
