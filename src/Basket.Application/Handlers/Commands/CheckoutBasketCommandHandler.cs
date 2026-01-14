using Basket.Application.Commands;
using Basket.Application.Common.Interfaces;
using Basket.Application.Repositories;

namespace Basket.Application.Handlers.Commands;

public class CheckoutBasketCommandHandler(
    IBasketRepository basketRepository,
    IEventBus eventBus) : ICommandHandler<CheckoutBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository = basketRepository;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        // Get the basket
        var basket = await _basketRepository.GetBasketAsync(request.BasketCheckout.UserName);

        if (basket == null || basket.IsEmpty)
        {
            return false;
        }

        // Create and publish the domain event
        var checkoutEvent = basket.CreateCheckoutEvent(
            firstName: request.BasketCheckout.FirstName,
            lastName: request.BasketCheckout.LastName,
            emailAddress: request.BasketCheckout.EmailAddress,
            addressLine: request.BasketCheckout.AddressLine,
            country: request.BasketCheckout.Country,
            state: request.BasketCheckout.State,
            zipCode: request.BasketCheckout.ZipCode,
            cardName: request.BasketCheckout.CardName,
            cardNumber: request.BasketCheckout.CardNumber,
            expiration: request.BasketCheckout.Expiration,
            cvv: request.BasketCheckout.CVV,
            paymentMethod: request.BasketCheckout.PaymentMethod);

        await _eventBus.PublishAsync(checkoutEvent, cancellationToken);

        // Delete the basket after checkout
        await _basketRepository.DeleteBasketAsync(request.BasketCheckout.UserName);

        return true;
    }
}
