using Basket.Application.Commands;
using Basket.Application.Common.Interfaces;
using Basket.Application.DTOs;
using Basket.Application.Mappings;
using Basket.Application.Repositories;

namespace Basket.Application.Handlers.Commands;

public class CreateBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<CreateBasketCommand, ShoppingCartDto>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<ShoppingCartDto> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = request.Basket.ToDomain();
        var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
        return updatedBasket.ToDto();
    }
}
