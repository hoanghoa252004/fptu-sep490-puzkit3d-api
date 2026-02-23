using PuzKit3D.Contract.Abstractions.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.Abstractions.Shared.Results.Validation;

public sealed class ValidationResult : Result
{
    private ValidationResult(Error[] errors)
        : base(
            isSuccess: false,
            error: Error.Validation("Validation.Failed", "One or more validation errors occurred.")
        )
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    // Factory methods
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult<TValue> : Result<TValue>
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        : base(
            default, 
            false, 
            Error.Validation("Validation.Failed", "One or more validation errors occurred.")
        )
    => Errors = errors;

    // Factory methods
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}
