using Basket.Application.Common.Interfaces;
using Basket.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.EventBus;

public class EventBus(IPublishEndpoint publishEndpoint, ILogger<EventBus> logger) : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<EventBus> _logger = logger;

    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        _logger.LogInformation("Publishing domain event: {EventType} - {Event}", typeof(T).Name, domainEvent);

        await _publishEndpoint.Publish(domainEvent, cancellationToken);

        _logger.LogInformation("Domain event published successfully: {EventType}", typeof(T).Name);
    }
}
