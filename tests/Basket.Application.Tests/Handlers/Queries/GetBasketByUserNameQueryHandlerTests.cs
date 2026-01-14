using Basket.Application.Handlers.Queries;
using Basket.Application.Queries;
using Basket.Application.Repositories;
using Basket.Domain;
using Moq;

namespace Basket.Application.Tests.Handlers.Queries;

public class GetBasketByUserNameQueryHandlerTests
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly GetBasketByUserNameQueryHandler _handler;

    public GetBasketByUserNameQueryHandlerTests()
    {
        _mockRepository = new Mock<IBasketRepository>();
        _handler = new GetBasketByUserNameQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Given_ExistingBasket_When_HandlingQuery_Then_ShouldReturnBasketDto()
    {
        // Arrange
        var userName = "testuser";
        var basket = new ShoppingCart(userName);
        basket.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));

        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync(basket);

        var query = new GetBasketByUserNameQuery(userName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
        Assert.Single(result.Items);
        Assert.Equal(TestData.ProductId1, result.Items[0].ProductId);
        _mockRepository.Verify(x => x.GetBasketAsync(userName), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentBasket_When_HandlingQuery_Then_ShouldReturnNull()
    {
        // Arrange
        var userName = "nonexistent";
        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync((ShoppingCart?)null);

        var query = new GetBasketByUserNameQuery(userName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(x => x.GetBasketAsync(userName), Times.Once);
    }

    [Fact]
    public async Task Given_EmptyBasket_When_HandlingQuery_Then_ShouldReturnBasketDtoWithNoItems()
    {
        // Arrange
        var userName = "testuser";
        var basket = new ShoppingCart(userName);

        _mockRepository.Setup(x => x.GetBasketAsync(userName))
            .ReturnsAsync(basket);

        var query = new GetBasketByUserNameQuery(userName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
        Assert.Empty(result.Items);
        Assert.Equal(0m, result.TotalPrice);
    }
}
