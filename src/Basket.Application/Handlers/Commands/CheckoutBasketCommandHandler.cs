using Basket.Application.Commands;
using Basket.Application.Common.Interfaces;
using Basket.Application.Repositories;

namespace Basket.Application.Handlers.Commands;

public class CheckoutBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<CheckoutBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        // Get the basket
        var basket = await _basketRepository.GetBasketAsync(request.BasketCheckout.UserName);

        if (basket == null || basket.IsEmpty)
        {
            return false;
        }

        // TODO: Publish BasketCheckoutEvent to EventBus (will be implemented in Infrastructure layer)
        // For now, just delete the basket after checkout
        await _basketRepository.DeleteBasketAsync(request.BasketCheckout.UserName);

        return true;
    }
}
