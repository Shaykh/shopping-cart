namespace Basket.Domain.Exceptions;

public class InvalidProductNameException : DomainException
{
    public InvalidProductNameException() : base("Product name cannot be null or empty.")
    {
    }
}
