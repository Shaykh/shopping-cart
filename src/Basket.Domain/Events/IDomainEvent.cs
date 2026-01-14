namespace Basket.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
