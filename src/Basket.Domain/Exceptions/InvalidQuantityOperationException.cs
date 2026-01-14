namespace Basket.Domain.Exceptions;

public class InvalidQuantityOperationException(string message) : DomainException(message)
{
}
