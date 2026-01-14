using Basket.Application.DTOs;
using FluentValidation;

namespace Basket.Application.Validators;

public class ShoppingCartDtoValidator : AbstractValidator<ShoppingCartDto>
{
    public ShoppingCartDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .NotNull().WithMessage("User name cannot be null.");

        RuleForEach(x => x.Items)
            .SetValidator(new CartItemDtoValidator());
    }
}
