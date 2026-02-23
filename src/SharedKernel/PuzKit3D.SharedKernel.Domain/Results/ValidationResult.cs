using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Domain.Results;

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