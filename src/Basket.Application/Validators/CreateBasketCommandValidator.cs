using Basket.Application.Commands;
using FluentValidation;

namespace Basket.Application.Validators;

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.Basket)
            .NotNull().WithMessage("Basket cannot be null.");

        RuleFor(x => x.Basket)
            .SetValidator(new ShoppingCartDtoValidator())
            .When(x => x.Basket != null);
    }
}
