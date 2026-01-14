using Basket.Application.Commands;
using FluentValidation;

namespace Basket.Application.Validators;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .NotNull().WithMessage("User name cannot be null.");
    }
}
