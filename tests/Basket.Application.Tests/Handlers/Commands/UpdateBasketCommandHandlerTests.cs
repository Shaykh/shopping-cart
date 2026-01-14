using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Handlers.Commands;
using Basket.Application.Repositories;
using Basket.Domain;
using Moq;

namespace Basket.Application.Tests.Handlers.Commands;

public class UpdateBasketCommandHandlerTests
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly UpdateBasketCommandHandler _handler;

    public UpdateBasketCommandHandlerTests()
    {
        _mockRepository = new Mock<IBasketRepository>();
        _handler = new UpdateBasketCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Given_ValidBasketDto_When_HandlingCommand_Then_ShouldUpdateAndReturnBasketDto()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "testuser",
            Items = new List<CartItemDto>
            {
                new CartItemDto
                {
                    ProductId = TestData.ProductId1,
                    ProductName = "Product 1",
                    Price = 10.00m,
                    Quantity = 5
                }
            }
        };

        var updatedBasket = new ShoppingCart("testuser");
        updatedBasket.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 5));

        _mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()))
            .ReturnsAsync(updatedBasket);

        var command = new UpdateBasketCommand(basketDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(basketDto.UserName, result.UserName);
        Assert.Single(result.Items);
        Assert.Equal(5, result.Items[0].Quantity);
        Assert.Equal(50.00m, result.TotalPrice);
        _mockRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()), Times.Once);
    }

    [Fact]
    public async Task Given_EmptyBasketDto_When_HandlingCommand_Then_ShouldUpdateToEmptyBasket()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "testuser",
            Items = new List<CartItemDto>()
        };

        var updatedBasket = new ShoppingCart("testuser");

        _mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()))
            .ReturnsAsync(updatedBasket);

        var command = new UpdateBasketCommand(basketDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0m, result.TotalPrice);
    }
}
