using Basket.Application.Queries;
using FluentValidation;

namespace Basket.Application.Validators;

public class GetBasketByUserNameQueryValidator : AbstractValidator<GetBasketByUserNameQuery>
{
    public GetBasketByUserNameQueryValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .NotNull().WithMessage("User name cannot be null.");
    }
}
