using Basket.Application.DTOs;
using Basket.Domain;

namespace Basket.Application.Mappings;

public static class MappingExtensions
{
    public static ShoppingCartDto ToDto(this ShoppingCart cart)
    {
        return new ShoppingCartDto
        {
            UserName = cart.UserName,
            Items = [.. cart.Items.Select(item => item.ToDto())],
            TotalPrice = cart.TotalPrice
        };
    }

    public static CartItemDto ToDto(this CartItem item)
    {
        return new CartItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Price = item.Price,
            Quantity = item.Quantity,
            Color = item.Color
        };
    }

    public static ShoppingCart ToDomain(this ShoppingCartDto dto)
    {
        var cart = new ShoppingCart(dto.UserName);
        foreach (var itemDto in dto.Items)
        {
            var item = new CartItem(
                itemDto.ProductId,
                itemDto.ProductName,
                itemDto.Price,
                itemDto.Quantity,
                itemDto.Color);
            cart.AddItem(item);
        }
        return cart;
    }
}
