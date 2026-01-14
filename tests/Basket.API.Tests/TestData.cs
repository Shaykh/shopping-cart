using Basket.Application.DTOs;

namespace Basket.API.Tests;

public static class TestData
{
    public static readonly Guid ProductId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ProductId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static ShoppingCartDto CreateShoppingCartDto(string userName = "testuser")
    {
        return new ShoppingCartDto
        {
            UserName = userName,
            Items =
            [
                new CartItemDto
                {
                    ProductId = ProductId1,
                    ProductName = "Product 1",
                    Price = 10.50m,
                    Quantity = 2,
                    Color = "Red"
                },
                new CartItemDto
                {
                    ProductId = ProductId2,
                    ProductName = "Product 2",
                    Price = 20.00m,
                    Quantity = 1,
                    Color = "Blue"
                }
            ],
            TotalPrice = 41.00m
        };
    }

    public static BasketCheckoutDto CreateBasketCheckoutDto(string userName = "testuser")
    {
        return new BasketCheckoutDto
        {
            UserName = userName,
            TotalPrice = 41.00m,
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = "john.doe@example.com",
            AddressLine = "123 Main St",
            Country = "USA",
            State = "CA",
            ZipCode = "12345",
            CardName = "John Doe",
            CardNumber = "1234567890123456",
            Expiration = "12/25",
            CVV = "123",
            PaymentMethod = 1
        };
    }
}
