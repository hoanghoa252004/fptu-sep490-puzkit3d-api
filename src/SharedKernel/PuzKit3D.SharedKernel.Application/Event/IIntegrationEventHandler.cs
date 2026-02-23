using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Event;

/// <summary>
/// Handler interface for integration events
/// </summary>
public interface IIntegrationEventHandler<in TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Handles the integration event
    /// </summary>
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}