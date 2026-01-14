using Basket.Domain.Events;

namespace Basket.Application.Common.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
