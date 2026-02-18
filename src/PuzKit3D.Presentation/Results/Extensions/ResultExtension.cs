using Microsoft.AspNetCore.Http;
using PuzKit3D.Contract.Abstractions.Shared.Errors;
using PuzKit3D.Contract.Abstractions.Shared.Results;

namespace PuzKit3D.Presentation.Results.Extensions;

public static class ResultExtension
{
    /// <summary>
    /// Match Result to HTTP response (success: 200 OK, failure: Problem)
    /// </summary>
    public static IResult MatchOk(this Result result)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.Ok()
            : result.Problem();
    }

    /// <summary>
    /// Match Result<T> to HTTP response (success: 200 OK with value, failure: Problem)
    /// </summary>
    public static IResult MatchOk<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.Ok(result.Value)
            : result.Problem();
    }

    /// <summary>
    /// Match Result to HTTP response with custom success response
    /// </summary>
    public static IResult MatchOk(
        this Result result,
        Func<IResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess()
            : result.Problem();
    }

    /// <summary>
    /// Match Result<T> to HTTP response with custom success response
    /// </summary>
    public static IResult MatchOk<T>(
        this Result<T> result,
        Func<T, IResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : result.Problem();
    }

    /// <summary>
    /// Match Result<T> to CreatedAtRoute response
    /// </summary>
    public static IResult MatchCreated<T>(
        this Result<T> result,
        string routeName,
        Func<T, object> routeValues)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.CreatedAtRoute(routeName, routeValues(result.Value), result.Value)
            : result.Problem();
    }

    /// <summary>
    /// Match Result<T> to CreatedAtRoute response with fixed route values
    /// </summary>
    public static IResult MatchCreated<T>(
        this Result<T> result,
        string routeName,
        object routeValues)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.CreatedAtRoute(routeName, routeValues, result.Value)
            : result.Problem();
    }

    /// <summary>
    /// Match Result to NoContent response
    /// </summary>
    public static IResult MatchNoContent(this Result result)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.NoContent()
            : result.Problem();
    }

    /// <summary>
    /// Match Result to Accepted response
    /// </summary>
    public static IResult MatchAccepted<T>(
        this Result<T> result,
        string? uri = null)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.Accepted(uri, result.Value)
            : result.Problem();
    }

    /// <summary>
    /// Match with different error handling based on ErrorType
    /// </summary>
    public static IResult MatchWithErrorHandling<T>(
        this Result<T> result,
        Func<T, IResult> onSuccess,
        Func<Error, IResult>? onError = null)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value);
        }

        return onError?.Invoke(result.Error) ?? result.Problem();
    }
}