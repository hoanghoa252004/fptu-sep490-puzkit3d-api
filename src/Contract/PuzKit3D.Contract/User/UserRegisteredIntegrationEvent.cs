using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.User;

public sealed record UserRegisteredIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    string Email,
    string UserId,
    string Token
) : IIntegrationEvent;
