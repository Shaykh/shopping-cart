using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Handlers.Commands;
using Basket.Application.Repositories;
using Basket.Domain;
using Moq;

namespace Basket.Application.Tests.Handlers.Commands;

public class CreateBasketCommandHandlerTests
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly CreateBasketCommandHandler _handler;

    public CreateBasketCommandHandlerTests()
    {
        _mockRepository = new Mock<IBasketRepository>();
        _handler = new CreateBasketCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Given_ValidBasketDto_When_HandlingCommand_Then_ShouldCreateAndReturnBasketDto()
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
                    Quantity = 2
                }
            }
        };

        var savedBasket = new ShoppingCart("testuser");
        savedBasket.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));

        _mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()))
            .ReturnsAsync(savedBasket);

        var command = new CreateBasketCommand(basketDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(basketDto.UserName, result.UserName);
        Assert.Single(result.Items);
        Assert.Equal(20.00m, result.TotalPrice);
        _mockRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()), Times.Once);
    }

    [Fact]
    public async Task Given_BasketWithMultipleItems_When_HandlingCommand_Then_ShouldCreateBasketWithAllItems()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "testuser",
            Items = new List<CartItemDto>
            {
                new CartItemDto { ProductId = TestData.ProductId1, ProductName = "Product 1", Price = 10.00m, Quantity = 2 },
                new CartItemDto { ProductId = TestData.ProductId2, ProductName = "Product 2", Price = 15.00m, Quantity = 3 }
            }
        };

        var savedBasket = new ShoppingCart("testuser");
        savedBasket.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));
        savedBasket.AddItem(new CartItem(TestData.ProductId2, "Product 2", 15.00m, 3));

        _mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<ShoppingCart>()))
            .ReturnsAsync(savedBasket);

        var command = new CreateBasketCommand(basketDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(65.00m, result.TotalPrice); // (10*2) + (15*3) = 20 + 45 = 65
    }
}
