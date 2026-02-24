using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommandT<AuthenticationResult>;