using Basket.Domain;

namespace Basket.Application.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasketAsync(string userName);
    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
    Task<bool> DeleteBasketAsync(string userName);
}
