using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Event;

/// <summary>
/// Event bus for publishing integration events across modules
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event to all subscribed handlers
    /// </summary>
    Task PublishAsync<TIntegrationEvent>(
        TIntegrationEvent @event,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent;
}