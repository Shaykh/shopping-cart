namespace Basket.Domain.Exceptions;

public class InvalidUserNameException : DomainException
{
    public InvalidUserNameException() : base("User name cannot be null or empty.")
    {
    }
}
