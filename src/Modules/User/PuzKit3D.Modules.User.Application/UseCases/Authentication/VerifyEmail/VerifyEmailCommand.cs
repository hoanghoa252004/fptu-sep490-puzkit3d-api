using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.VerifyEmail;

public sealed record VerifyEmailCommand(string UserId, string Token) : ICommand;
