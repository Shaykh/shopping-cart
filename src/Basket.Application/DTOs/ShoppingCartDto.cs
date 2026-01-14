namespace Basket.Application.DTOs;

public class ShoppingCartDto
{
    public string UserName { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
}
