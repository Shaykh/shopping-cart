namespace Basket.Domain.Exceptions;

public class CartItemNotFoundException(Guid productId) : DomainException($"Item with ProductId '{productId}' not found in cart.")
{
}
