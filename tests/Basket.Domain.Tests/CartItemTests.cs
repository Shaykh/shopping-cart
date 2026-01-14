namespace Basket.Domain.Tests;

public class CartItemTests
{
    [Fact]
    public void Given_ValidParameters_When_ConstructingCartItem_Then_ShouldCreateCartItem()
    {
        // Arrange & Act
        var productId = Guid.NewGuid();
        var cartItem = new CartItem(productId, "Product Name", 10.50m, 2, "Red");

        // Assert
        Assert.Equal(productId, cartItem.ProductId);
        Assert.Equal("Product Name", cartItem.ProductName);
        Assert.Equal(10.50m, cartItem.Price);
        Assert.Equal(2, cartItem.Quantity);
        Assert.Equal("Red", cartItem.Color);
    }

    [Fact]
    public void Given_ValidParametersWithoutColor_When_ConstructingCartItem_Then_ShouldCreateCartItem()
    {
        // Arrange & Act
        var productId = Guid.NewGuid();
        var cartItem = new CartItem(productId, "Product Name", 10.50m, 2);

        // Assert
        Assert.Equal(productId, cartItem.ProductId);
        Assert.Null(cartItem.Color);
    }

    [Fact]
    public void Given_EmptyProductId_When_ConstructingCartItem_Then_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new CartItem(Guid.Empty, "Product Name", 10.50m, 2));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_InvalidProductName_When_ConstructingCartItem_Then_ShouldThrowArgumentException(string? productName)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new CartItem(Guid.NewGuid(), productName, 10.50m, 2));
    }

    [Fact]
    public void Given_NegativePrice_When_ConstructingCartItem_Then_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new CartItem(Guid.NewGuid(), "Product Name", -10.50m, 2));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_InvalidQuantity_When_ConstructingCartItem_Then_ShouldThrowArgumentException(int quantity)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new CartItem(Guid.NewGuid(), "Product Name", 10.50m, quantity));
    }

    [Fact]
    public void Given_CartItemWithPriceAndQuantity_When_GettingTotalPrice_Then_ShouldCalculateCorrectly()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 3);

        // Act
        var totalPrice = cartItem.TotalPrice;

        // Assert
        Assert.Equal(31.50m, totalPrice);
    }

    [Fact]
    public void Given_CartItem_When_UpdatingQuantityWithValidValue_Then_ShouldUpdateQuantity()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 2);

        // Act
        cartItem.UpdateQuantity(5);

        // Assert
        Assert.Equal(5, cartItem.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_CartItem_When_UpdatingQuantityWithInvalidValue_Then_ShouldThrowArgumentException(int quantity)
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cartItem.UpdateQuantity(quantity));
    }

    [Fact]
    public void Given_CartItem_When_IncreasingQuantityWithValidAmount_Then_ShouldIncreaseQuantity()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 2);

        // Act
        cartItem.IncreaseQuantity(3);

        // Assert
        Assert.Equal(5, cartItem.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_CartItem_When_IncreasingQuantityWithInvalidAmount_Then_ShouldThrowArgumentException(int amount)
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cartItem.IncreaseQuantity(amount));
    }

    [Fact]
    public void Given_CartItem_When_DecreasingQuantityWithValidAmount_Then_ShouldDecreaseQuantity()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 5);

        // Act
        cartItem.DecreaseQuantity(2);

        // Assert
        Assert.Equal(3, cartItem.Quantity);
    }

    [Fact]
    public void Given_CartItem_When_DecreasingQuantityThatWouldResultInNegative_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 2);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cartItem.DecreaseQuantity(5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_CartItem_When_DecreasingQuantityWithInvalidAmount_Then_ShouldThrowArgumentException(int amount)
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), "Product Name", 10.50m, 5);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cartItem.DecreaseQuantity(amount));
    }
}
