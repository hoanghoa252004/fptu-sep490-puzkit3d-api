using FluentValidation;
using MediatR;
using PuzKit3D.Contract.Abstractions.Shared.Errors;
using PuzKit3D.Contract.Abstractions.Shared.Errors.Validation;
using PuzKit3D.Contract.Abstractions.Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ValidationResult = PuzKit3D.Contract.Abstractions.Shared.Results.Validation.ValidationResult;

namespace PuzKit3D.Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToArray();

        if (failures.Length == 0)
        {
            return await next();
        }

        return CreateValidationResult<TResponse>(failures);

    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        var validationError = new ValidationError(errors);

        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(validationError) as TResult)!;
        }

        // ✅ Case 2: Result<T> (generic)
        // Get generic argument type (e.g., Guid, string, etc.)
        var resultType = typeof(TResult).GenericTypeArguments[0];

        // ✅ FIX: Specify generic parameter count and parameter types
        var failureMethod = typeof(Result)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.Failure))
            .Where(m => m.IsGenericMethodDefinition) // ✅ Only generic method
            .Where(m => m.GetGenericArguments().Length == 1) // ✅ Has 1 generic param
            .Single(); // Should find exactly 1 method

        // Make generic method: Failure<T>
        var genericMethod = failureMethod.MakeGenericMethod(resultType);

        // Invoke: Result.Failure<T>(validationError)
        var result = genericMethod.Invoke(null, new object[] { validationError });

        return (TResult)result!;
    }
}
