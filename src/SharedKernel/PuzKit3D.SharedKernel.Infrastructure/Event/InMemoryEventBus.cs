using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Event;

/// <summary>
/// In-memory event bus for integration events (for modular monolith)
/// </summary>
public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(
        IServiceProvider serviceProvider,
        ILogger<InMemoryEventBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishAsync<TIntegrationEvent>(
        TIntegrationEvent @event,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        _logger.LogInformation(
            "Publishing integration event {EventType} with ID {EventId}",
            @event.GetType().Name,
            @event.Id);

        using var scope = _serviceProvider.CreateScope();

        // Get all handlers for this event type
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        // Execute all handlers
        var tasks = handlers.Select(handler =>
        {
            var handleMethod = handlerType.GetMethod(nameof(IIntegrationEventHandler<TIntegrationEvent>.HandleAsync));
            return (Task)handleMethod!.Invoke(handler, new object[] { @event, cancellationToken })!;
        });

        await Task.WhenAll(tasks);

        _logger.LogInformation(
            "Published integration event {EventType} to {HandlerCount} handlers",
            @event.GetType().Name,
            handlers.Count());
    }
}
