using Basket.Domain;
using Basket.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Infrastructure.Tests.Repositories;

public class BasketRepositoryTests
{
    private readonly Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
    private readonly Mock<IDatabase> _mockDatabase;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly BasketRepository _repository;

    public BasketRepositoryTests()
    {
        _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
        _mockDatabase = new Mock<IDatabase>();
        _mockConfiguration = new Mock<IConfiguration>();

        _mockConnectionMultiplexer
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_mockDatabase.Object);

        _repository = new BasketRepository(_mockConnectionMultiplexer.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task Given_ExistingBasket_When_GettingBasket_Then_ShouldReturnDeserializedBasket()
    {
        // Arrange
        var userName = "testuser";
        var cart = TestData.CreateShoppingCart(userName);
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            IncludeFields = true
        };
        jsonOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        var serializedBasket = JsonSerializer.Serialize(cart, jsonOptions);
        var redisValue = RedisValue.Unbox(serializedBasket);

        _mockDatabase
            .Setup(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()))
            .ReturnsAsync(redisValue);

        // Act
        var result = await _repository.GetBasketAsync(userName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
        Assert.Single(result.Items);
        Assert.Equal(20.00m, result.TotalPrice);
        _mockDatabase.Verify(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentBasket_When_GettingBasket_Then_ShouldReturnNull()
    {
        // Arrange
        var userName = "nonexistent";
        var redisValue = RedisValue.Null;

        _mockDatabase
            .Setup(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()))
            .ReturnsAsync(redisValue);

        // Act
        var result = await _repository.GetBasketAsync(userName);

        // Assert
        Assert.Null(result);
        _mockDatabase.Verify(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Given_EmptyBasket_When_GettingBasket_Then_ShouldReturnNull()
    {
        // Arrange
        var userName = "emptyuser";
        var redisValue = RedisValue.EmptyString;

        _mockDatabase
            .Setup(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()))
            .ReturnsAsync(redisValue);

        // Act
        var result = await _repository.GetBasketAsync(userName);

        // Assert
        Assert.Null(result);
        _mockDatabase.Verify(x => x.StringGetAsync(userName, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Given_ValidBasket_When_UpdatingBasket_Then_ShouldSerializeAndStoreBasket()
    {
        // Arrange
        var cart = TestData.CreateShoppingCart("testuser");

        _mockDatabase
            .Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == cart.UserName),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        var result = await _repository.UpdateBasketAsync(cart);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cart.UserName, result.UserName);
        Assert.Equal(cart.Items.Count, result.Items.Count);
        // Verify that StringSetAsync was called (StackExchange.Redis uses extension methods)
        // We verify by checking the setup was invoked
        _mockDatabase.Verify(
            x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_EmptyBasket_When_UpdatingBasket_Then_ShouldStoreEmptyBasket()
    {
        // Arrange
        var cart = TestData.CreateEmptyShoppingCart("testuser");

        _mockDatabase
            .Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == cart.UserName),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        var result = await _repository.UpdateBasketAsync(cart);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cart.UserName, result.UserName);
        Assert.Empty(result.Items);
        // Verify that StringSetAsync was called (StackExchange.Redis uses extension methods)
        // We verify by checking the setup was invoked
        _mockDatabase.Verify(
            x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_ExistingBasket_When_DeletingBasket_Then_ShouldReturnTrue()
    {
        // Arrange
        var userName = "testuser";

        _mockDatabase
            .Setup(x => x.KeyDeleteAsync(userName, It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        var result = await _repository.DeleteBasketAsync(userName);

        // Assert
        Assert.True(result);
        _mockDatabase.Verify(x => x.KeyDeleteAsync(userName, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentBasket_When_DeletingBasket_Then_ShouldReturnFalse()
    {
        // Arrange
        var userName = "nonexistent";

        _mockDatabase
            .Setup(x => x.KeyDeleteAsync(userName, It.IsAny<CommandFlags>()))
            .ReturnsAsync(false);

        // Act
        var result = await _repository.DeleteBasketAsync(userName);

        // Assert
        Assert.False(result);
        _mockDatabase.Verify(x => x.KeyDeleteAsync(userName, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Given_BasketWithMultipleItems_When_UpdatingBasket_Then_ShouldSerializeAllItems()
    {
        // Arrange
        var cart = new ShoppingCart("testuser");
        cart.AddItem(new CartItem(TestData.ProductId1, "Product 1", 10.00m, 2));
        cart.AddItem(new CartItem(TestData.ProductId2, "Product 2", 15.00m, 3));

        _mockDatabase
            .Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == cart.UserName),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        var result = await _repository.UpdateBasketAsync(cart);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(65.00m, result.TotalPrice); // (10 * 2) + (15 * 3) = 20 + 45 = 65
        // Verify that StringSetAsync was called (StackExchange.Redis uses extension methods)
        // We verify by checking the setup was invoked
        _mockDatabase.Verify(
            x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }
}
