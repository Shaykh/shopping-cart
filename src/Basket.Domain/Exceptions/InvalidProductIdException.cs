namespace Basket.Domain.Exceptions;

public class InvalidProductIdException : DomainException
{
    public InvalidProductIdException() : base("Product ID cannot be empty.")
    {
    }
}
