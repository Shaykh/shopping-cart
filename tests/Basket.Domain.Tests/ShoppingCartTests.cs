using Basket.Domain.Exceptions;

namespace Basket.Domain.Tests;

public class ShoppingCartTests
{
    [Fact]
    public void Given_ValidUserName_When_ConstructingShoppingCart_Then_ShouldCreateShoppingCart()
    {
        // Arrange & Act
        var cart = new ShoppingCart("testuser");

        // Assert
        Assert.Equal("testuser", cart.UserName);
        Assert.NotNull(cart.Items);
        Assert.Empty(cart.Items);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_InvalidUserName_When_ConstructingShoppingCart_Then_ShouldThrowInvalidUserNameException(string? userName)
    {
        // Arrange, Act & Assert
        Assert.Throws<InvalidUserNameException>(() => new ShoppingCart(userName));
    }

    [Fact]
    public void Given_EmptyCart_When_CheckingIsEmpty_Then_ShouldReturnTrue()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.True(cart.IsEmpty);
    }

    [Fact]
    public void Given_CartWithItems_When_CheckingIsEmpty_Then_ShouldReturnFalse()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1);

        // Act
        cart.AddItem(item);

        // Assert
        Assert.False(cart.IsEmpty);
    }

    [Fact]
    public void Given_EmptyCart_When_AddingNewItem_Then_ShouldAddItemToCart()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1);

        // Act
        cart.AddItem(item);

        // Assert
        Assert.Single(cart.Items);
        Assert.Equal(TestData.ProductId1, cart.Items[0].ProductId);
    }

    [Fact]
    public void Given_EmptyCart_When_AddingNullItem_Then_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cart.AddItem(null!));
    }

    [Fact]
    public void Given_CartWithExistingProduct_When_AddingSameProduct_Then_ShouldIncreaseQuantity()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item1 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2);
        var item2 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 3);

        // Act
        cart.AddItem(item1);
        cart.AddItem(item2);

        // Assert
        Assert.Single(cart.Items);
        Assert.Equal(5, cart.Items[0].Quantity);
    }

    [Fact]
    public void Given_CartWithProduct_When_AddingSameProductWithDifferentColor_Then_ShouldAddAsSeparateItem()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item1 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2, "Red");
        var item2 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 3, "Blue");

        // Act
        cart.AddItem(item1);
        cart.AddItem(item2);

        // Assert
        Assert.Equal(2, cart.Items.Count);
    }

    [Fact]
    public void Given_CartWithItem_When_RemovingExistingProductId_Then_ShouldRemoveItem()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1);
        cart.AddItem(item);

        // Act
        cart.RemoveItem(TestData.ProductId1);

        // Assert
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Given_EmptyCart_When_RemovingNonExistentProductId_Then_ShouldNotThrow()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act
        cart.RemoveItem(Guid.NewGuid());

        // Assert
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Given_CartWithItemsOfDifferentColors_When_RemovingItemWithColor_Then_ShouldRemoveSpecificItem()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item1 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1, "Red");
        var item2 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1, "Blue");
        cart.AddItem(item1);
        cart.AddItem(item2);

        // Act
        cart.RemoveItem(TestData.ProductId1, "Red");

        // Assert
        Assert.Single(cart.Items);
        Assert.Equal("Blue", cart.Items[0].Color);
    }

    [Fact]
    public void Given_Cart_When_RemovingItemWithEmptyProductId_Then_ShouldThrowInvalidProductIdException()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.Throws<InvalidProductIdException>(() => cart.RemoveItem(Guid.Empty));
    }

    [Fact]
    public void Given_CartWithItem_When_UpdatingItemQuantity_Then_ShouldUpdateQuantity()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2);
        cart.AddItem(item);

        // Act
        cart.UpdateItemQuantity(TestData.ProductId1, 5);

        // Assert
        Assert.Equal(5, cart.Items[0].Quantity);
    }

    [Fact]
    public void Given_EmptyCart_When_UpdatingNonExistentItemQuantity_Then_ShouldThrowCartItemNotFoundException()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<CartItemNotFoundException>(() => cart.UpdateItemQuantity(productId, 5));
    }

    [Fact]
    public void Given_Cart_When_UpdatingItemQuantityWithEmptyProductId_Then_ShouldThrowInvalidProductIdException()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.Throws<InvalidProductIdException>(() => cart.UpdateItemQuantity(Guid.Empty, 5));
    }

    [Fact]
    public void Given_CartWithItemsOfDifferentColors_When_UpdatingItemQuantityWithColor_Then_ShouldUpdateSpecificItem()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        var item1 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2, "Red");
        var item2 = new CartItem(TestData.ProductId1, "Product 1", 10.00m, 3, "Blue");
        cart.AddItem(item1);
        cart.AddItem(item2);

        // Act
        cart.UpdateItemQuantity(TestData.ProductId1, 5, "Red");

        // Assert
        var redItem = cart.Items.First(x => x.Color == "Red");
        Assert.Equal(5, redItem.Quantity);
        Assert.Equal(3, cart.Items.First(x => x.Color == "Blue").Quantity);
    }

    [Fact]
    public void Given_CartWithItems_When_ClearingCart_Then_ShouldRemoveAllItems()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 1));
        cart.AddItem(new CartItem(TestData.ProductId2, "Product 2", 20.00m, 2));

        // Act
        cart.Clear();

        // Assert
        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
    }

    [Fact]
    public void Given_EmptyCart_When_GettingTotalPrice_Then_ShouldReturnZero()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.Equal(0m, cart.TotalPrice);
    }

    [Fact]
    public void Given_CartWithSingleItem_When_GettingTotalPrice_Then_ShouldReturnItemTotal()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));

        // Act & Assert
        Assert.Equal(20.00m, cart.TotalPrice);
    }

    [Fact]
    public void Given_CartWithMultipleItems_When_GettingTotalPrice_Then_ShouldReturnSumOfAllItems()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));
        cart.AddItem(new CartItem(TestData.ProductId2, "Product 2", 15.00m, 3));
        cart.AddItem(new CartItem(TestData.ProductId3, "Product 3", 5.00m, 1));

        // Act & Assert
        // Expected: (10 * 2) + (15 * 3) + (5 * 1) = 20 + 45 + 5 = 70
        Assert.Equal(70.00m, cart.TotalPrice);
    }

    [Fact]
    public void Given_CartWithItem_When_UpdatingQuantity_Then_TotalPriceShouldRecalculate()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));

        // Act
        cart.UpdateItemQuantity(TestData.ProductId1, 5);

        // Assert
        Assert.Equal(50.00m, cart.TotalPrice);
    }

    [Fact]
    public void Given_ShoppingCart_When_CreatingCheckoutEvent_Then_ShouldCreateEventWithAllData()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));
        cart.AddItem(new CartItem(TestData.ProductId2, "Product 2", 15.00m, 3));

        // Act
        var checkoutEvent = cart.CreateCheckoutEvent(
            firstName: "John",
            lastName: "Doe",
            emailAddress: "john.doe@example.com",
            addressLine: "123 Main St",
            country: "USA",
            state: "CA",
            zipCode: "12345",
            cardName: "John Doe",
            cardNumber: "1234567890123456",
            expiration: "12/25",
            cvv: "123",
            paymentMethod: 1);

        // Assert
        Assert.NotNull(checkoutEvent);
        Assert.Equal("testuser", checkoutEvent.UserName);
        Assert.Equal(65.00m, checkoutEvent.TotalPrice); // (10*2) + (15*3) = 20 + 45 = 65
        Assert.Equal("John", checkoutEvent.FirstName);
        Assert.Equal("Doe", checkoutEvent.LastName);
        Assert.Equal("john.doe@example.com", checkoutEvent.EmailAddress);
        Assert.Equal(2, checkoutEvent.Items.Count);
        Assert.Equal(TestData.ProductId1, checkoutEvent.Items[0].ProductId);
        Assert.Equal(TestData.ProductId2, checkoutEvent.Items[1].ProductId);
        Assert.True(checkoutEvent.OccurredOn <= DateTime.UtcNow);
    }

    [Fact]
    public void Given_ShoppingCart_When_CheckingIsAggregateRoot_Then_ShouldImplementIAggregateRoot()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.IsAssignableFrom<Common.IAggregateRoot>(cart);
    }
}
