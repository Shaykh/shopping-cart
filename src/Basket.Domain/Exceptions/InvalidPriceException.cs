namespace Basket.Domain.Exceptions;

public class InvalidPriceException : DomainException
{
    public InvalidPriceException() : base("Price cannot be negative.")
    {
    }
}
