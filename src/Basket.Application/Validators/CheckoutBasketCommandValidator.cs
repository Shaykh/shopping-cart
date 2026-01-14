using Basket.Application.Commands;
using FluentValidation;

namespace Basket.Application.Validators;

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout)
            .NotNull().WithMessage("Basket checkout cannot be null.");

        RuleFor(x => x.BasketCheckout)
            .SetValidator(new BasketCheckoutDtoValidator())
            .When(x => x.BasketCheckout != null);
    }
}
