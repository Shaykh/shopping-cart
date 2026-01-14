namespace Basket.Domain.Events;

public record BasketCheckoutEvent(
    string UserName,
    decimal TotalPrice,
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode,
    string CardName,
    string CardNumber,
    string Expiration,
    string CVV,
    int PaymentMethod,
    List<BasketCheckoutItem> Items,
    DateTime OccurredOn) : IDomainEvent;

public record BasketCheckoutItem(
    Guid ProductId,
    string ProductName,
    decimal Price,
    int Quantity,
    string? Color = null);
