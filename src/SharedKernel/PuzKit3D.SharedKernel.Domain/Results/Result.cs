using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Domain.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Success result cannot have an error.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    // Factory methods
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static ResultT<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static ResultT<TValue> Failure<TValue>(Error error)
        => new ResultT<TValue>(default, false, error);
}
