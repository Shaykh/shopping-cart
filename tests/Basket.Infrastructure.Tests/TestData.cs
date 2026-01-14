using Basket.Domain;

namespace Basket.Infrastructure.Tests;

public static class TestData
{
    public static Guid ProductId1 => Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static Guid ProductId2 => Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static ShoppingCart CreateShoppingCart(string userName = "testuser")
    {
        var cart = new ShoppingCart(userName);
        cart.AddItem(new CartItem(ProductId1, "Product 1", 10.00m, 2));
        return cart;
    }

    public static ShoppingCart CreateEmptyShoppingCart(string userName = "testuser")
    {
        return new ShoppingCart(userName);
    }
}
