using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuzKit3D.Contract.Abstractions.Shared.Errors;
using PuzKit3D.Contract.Abstractions.Shared.Errors.Type;
using PuzKit3D.Contract.Abstractions.Shared.Errors.Validation;
using PuzKit3D.Contract.Abstractions.Shared.Results;

namespace PuzKit3D.Presentation.Results.Extensions;

/// <summary>
/// Extensions to create ProblemDetails from Result failures
/// </summary>
/// <summary>
/// Extensions to create ProblemDetails from Result failures
/// </summary>
public static class ProblemExtension
{
    /// <summary>
    /// Creates a Problem response from Result failure
    /// </summary>
    public static IResult Problem(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create problem from successful result.");
        }

        return Microsoft.AspNetCore.Http.Results.Problem(CreateProblemDetails(result.Error));
    }

    /// <summary>
    /// Creates a Problem response from Result<T> failure
    /// </summary>
    public static IResult Problem<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create problem from successful result.");
        }

        return Microsoft.AspNetCore.Http.Results.Problem(CreateProblemDetails(result.Error));
    }

    private static ProblemDetails CreateProblemDetails(Error error)
    {
        var statusCode = GetStatusCode(error.Type);

        // ✅ Handle ValidationError specially
        if (error is ValidationError validationError)
        {
            return CreateValidationProblemDetails(validationError, statusCode);
        }

        // Standard error
        return new ProblemDetails
        {
            Title = GetTitle(error.Type),
            Status = statusCode,
            Detail = error.Message,
            Type = GetTypeUrl(statusCode),
            Extensions =
            {
                ["errorCode"] = error.Code
            }
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        ValidationError validationError,
        int statusCode )
    {
        // Group validation errors by property/field
        var errorsDictionary = validationError.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );

        return new ValidationProblemDetails(errorsDictionary)
        {
            Title = "Validation Error",
            Status = statusCode,
            Detail = validationError.Message,
            Type = GetTypeUrl(statusCode)
        };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.Failure => StatusCodes.Status400BadRequest,
        ErrorType.None => StatusCodes.Status200OK,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Validation Error",
        ErrorType.NotFound => "Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        ErrorType.Failure => "Bad Request",
        _ => "Server Error"
    };

    private static string GetTypeUrl(int statusCode) =>
        $"https://httpstatuses.com/{statusCode}";
}