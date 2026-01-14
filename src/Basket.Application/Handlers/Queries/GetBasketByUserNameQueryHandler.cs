using Basket.Application.Common.Interfaces;
using Basket.Application.DTOs;
using Basket.Application.Mappings;
using Basket.Application.Queries;
using Basket.Application.Repositories;

namespace Basket.Application.Handlers.Queries;

public class GetBasketByUserNameQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketByUserNameQuery, ShoppingCartDto?>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<ShoppingCartDto?> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketAsync(request.UserName);
        return basket?.ToDto();
    }
}
