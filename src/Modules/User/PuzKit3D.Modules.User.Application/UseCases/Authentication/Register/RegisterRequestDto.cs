using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;

public sealed record RegisterRequestDto(
    string Email,
    string Password,
    string? FirstName,
    string? LastName
);
