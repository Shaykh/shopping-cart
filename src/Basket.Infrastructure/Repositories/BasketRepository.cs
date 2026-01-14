using Basket.Application.Repositories;
using Basket.Domain;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;

    public BasketRepository(IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _database = redis.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            IncludeFields = true
        };
        // Configure to handle private setters via reflection
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
        var basket = await _database.StringGetAsync(userName);

        if (basket.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<ShoppingCart>(basket.ToString(), _jsonOptions);
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
    {
        var serializedBasket = JsonSerializer.Serialize(basket, _jsonOptions);
        await _database.StringSetAsync(basket.UserName, serializedBasket);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName)
    {
        return await _database.KeyDeleteAsync(userName);
    }
}
