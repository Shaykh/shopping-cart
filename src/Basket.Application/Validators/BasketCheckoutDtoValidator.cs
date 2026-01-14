using Basket.Application.Constants;
using Basket.Application.DTOs;
using FluentValidation;

namespace Basket.Application.Validators;

public class BasketCheckoutDtoValidator : AbstractValidator<BasketCheckoutDto>
{
    public BasketCheckoutDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .NotNull().WithMessage("User name cannot be null.");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Address line is required.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required.");

        RuleFor(x => x.CardName)
            .NotEmpty().WithMessage("Card name is required.");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .Length(13, 19).WithMessage("Card number must be between 13 and 19 digits.");

        RuleFor(x => x.Expiration)
            .NotEmpty().WithMessage("Expiration date is required.")
            .Matches(RegularExpressions.ExpirationDateRegex).WithMessage("Expiration date must be in MM/YY format.");

        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage("CVV is required.")
            .Length(3, 4).WithMessage("CVV must be 3 or 4 digits.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0).WithMessage("Total price must be greater than zero.");
    }
}
