using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Handlers.Commands;
using Basket.Application.Repositories;
using Basket.Domain;
using Moq;

namespace Basket.Application.Tests.Handlers.Commands;

public class CheckoutBasketCommandHandlerTests
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly CheckoutBasketCommandHandler _handler;

    public CheckoutBasketCommandHandlerTests()
    {
        _mockRepository = new Mock<IBasketRepository>();
        _handler = new CheckoutBasketCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Given_ValidBasket_When_HandlingCheckout_Then_ShouldDeleteBasketAndReturnTrue()
    {
        // Arrange
        var userName = "testuser";
        var basket = new ShoppingCart(userName);
        basket.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));

        var checkoutDto = new BasketCheckoutDto
        {
            UserName = userName,
            TotalPrice = 20.00m,
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
            CVV = "123"
        };

        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync(basket);
        _mockRepository.Setup(x => x.DeleteBasketAsync(userName))
            .ReturnsAsync(true);

        var command = new CheckoutBasketCommand(checkoutDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(x => x.GetBasketAsync(userName), Times.Once);
        _mockRepository.Verify(x => x.DeleteBasketAsync(userName), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentBasket_When_HandlingCheckout_Then_ShouldReturnFalse()
    {
        // Arrange
        var userName = "nonexistent";
        var checkoutDto = new BasketCheckoutDto
        {
            UserName = userName,
            TotalPrice = 20.00m,
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
            CVV = "123"
        };

        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync((ShoppingCart?)null);

        var command = new CheckoutBasketCommand(checkoutDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(x => x.GetBasketAsync(userName), Times.Once);
        _mockRepository.Verify(x => x.DeleteBasketAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Given_EmptyBasket_When_HandlingCheckout_Then_ShouldReturnFalse()
    {
        // Arrange
        var userName = "testuser";
        var basket = new ShoppingCart(userName);

        var checkoutDto = new BasketCheckoutDto
        {
            UserName = userName,
            TotalPrice = 0m,
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
            CVV = "123"
        };

        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync(basket);

        var command = new CheckoutBasketCommand(checkoutDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(x => x.GetBasketAsync(userName), Times.Once);
        _mockRepository.Verify(x => x.DeleteBasketAsync(It.IsAny<string>()), Times.Never);
    }
}
