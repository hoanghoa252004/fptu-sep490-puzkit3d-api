using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.Delivery;

public sealed record OrderDeliveredIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId) : IIntegrationEvent;
