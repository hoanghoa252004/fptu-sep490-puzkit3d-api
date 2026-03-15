using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PuzKit3D.SharedKernel.Application.Exceptions;
using System.Net;

namespace PuzKit3D.WebApi.Middleware;

internal class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger, IHostEnvironment _env) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogException(exception, httpContext);

        var problemDetails = CreateProblemDetails(httpContext, exception);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status!.Value; // 500 Internal Server Error

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {

        var (statusCode, title, type) = exception switch
        {
            BadHttpRequestException =>
                (StatusCodes.Status400BadRequest,
                 "Bad Request",
                 "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),

            PuzKit3DException =>
                (StatusCodes.Status400BadRequest,
                 "Bad Request",
                 "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Server Failure",
                 "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = type,
            Title = title,
            Detail = _env.IsDevelopment()
                ? exception.Message
                : statusCode == StatusCodes.Status400BadRequest
                    ? "The request was malformed or invalid. Please check your input."
                    : "An unexpected error occurred. Please try again later or contact support.",
            Instance = context.Request.Path,
        };

        // Add trace ID for debugging
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        // Include details only in development
        if (problemDetails.Status == StatusCodes.Status500InternalServerError)
        {
            problemDetails.Extensions["exceptionType"] = exception.GetType().FullName;
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;

            if (exception.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = exception.InnerException.Message;
            }
        }

        if (problemDetails.Status == StatusCodes.Status400BadRequest)
        {
            problemDetails.Extensions["exceptionType"] = exception.GetType().FullName;

            if (exception.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = exception.InnerException.Message;
            }
        }

        return problemDetails;
    }

    private void LogException(Exception exception, HttpContext context)
    {
        var logLevel = exception switch
        {
            PuzKit3DException => LogLevel.Error, // Business logic errors
            _ => LogLevel.Warning // Unexpected errors, đã wrap rồi, nên sẽ ko bao giờ có
        };

        _logger.Log(
            logLevel,
            exception,
            "Exception occurred: {ExceptionType} at {Path}. TraceId: {TraceId}",
            exception.GetType().Name,
            context.Request.Path,
            context.TraceIdentifier
        );
    }
}