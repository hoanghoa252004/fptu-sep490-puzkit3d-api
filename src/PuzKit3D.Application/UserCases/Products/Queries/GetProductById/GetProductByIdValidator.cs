using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Application.UserCases.Products.Queries.GetProductById;

internal sealed class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(query => query.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0")

            .LessThanOrEqualTo(int.MaxValue)
            .WithMessage($"Id must be less than or equal to {int.MaxValue}")
            
            .NotEmpty()
            .WithMessage("Id can not be empty or null");
    }
}
