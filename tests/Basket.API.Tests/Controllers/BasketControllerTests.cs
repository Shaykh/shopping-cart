using Basket.API.Controllers;
using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Basket.API.Tests.Controllers;

public class BasketControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BasketController>> _mockLogger;
    private readonly BasketController _controller;

    public BasketControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BasketController>>();
        _controller = new BasketController(_mockMediator.Object, _mockLogger.Object);
    }

    #region GetBasket Tests

    [Fact]
    public async Task Given_ValidUserName_When_GetBasket_Then_ShouldReturnOkWithBasket()
    {
        // Arrange
        var userName = "testuser";
        var expectedBasket = TestData.CreateShoppingCartDto(userName);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBasket);

        // Act
        var result = await _controller.GetBasket(userName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var basket = Assert.IsType<ShoppingCartDto>(okResult.Value);
        Assert.Equal(userName, basket.UserName);
        Assert.Equal(2, basket.Items.Count);
        _mockMediator.Verify(m => m.Send(It.Is<GetBasketByUserNameQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentUserName_When_GetBasket_Then_ShouldReturnNotFound()
    {
        // Arrange
        var userName = "nonexistent";

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ShoppingCartDto?)null);

        // Act
        var result = await _controller.GetBasket(userName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains(userName, notFoundResult.Value?.ToString() ?? string.Empty);
        _mockMediator.Verify(m => m.Send(It.Is<GetBasketByUserNameQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_EmptyUserName_When_GetBasket_Then_ShouldCallMediator()
    {
        // Arrange
        var userName = string.Empty;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ShoppingCartDto?)null);

        // Act
        var result = await _controller.GetBasket(userName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        _mockMediator.Verify(m => m.Send(It.Is<GetBasketByUserNameQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateBasket Tests

    [Fact]
    public async Task Given_ValidBasketDto_When_UpdateBasket_Then_ShouldReturnOkWithUpdatedBasket()
    {
        // Arrange
        var basketDto = TestData.CreateShoppingCartDto();
        var expectedResult = TestData.CreateShoppingCartDto();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CreateBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.UpdateBasket(basketDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBasket = Assert.IsType<ShoppingCartDto>(okResult.Value);
        Assert.Equal(basketDto.UserName, returnedBasket.UserName);
        _mockMediator.Verify(m => m.Send(It.Is<CreateBasketCommand>(c => c.Basket == basketDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_BasketWithEmptyItems_When_UpdateBasket_Then_ShouldReturnOk()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "testuser",
            Items = [],
            TotalPrice = 0
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CreateBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(basketDto);

        // Act
        var result = await _controller.UpdateBasket(basketDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBasket = Assert.IsType<ShoppingCartDto>(okResult.Value);
        Assert.Empty(returnedBasket.Items);
        _mockMediator.Verify(m => m.Send(It.IsAny<CreateBasketCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteBasket Tests

    [Fact]
    public async Task Given_ValidUserName_When_DeleteBasket_Then_ShouldReturnOk()
    {
        // Arrange
        var userName = "testuser";

        _mockMediator
            .Setup(m => m.Send(It.IsAny<DeleteBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBasket(userName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        _mockMediator.Verify(m => m.Send(It.Is<DeleteBasketCommand>(c => c.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentUserName_When_DeleteBasket_Then_ShouldReturnNotFound()
    {
        // Arrange
        var userName = "nonexistent";

        _mockMediator
            .Setup(m => m.Send(It.IsAny<DeleteBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteBasket(userName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains(userName, notFoundResult.Value?.ToString() ?? string.Empty);
        _mockMediator.Verify(m => m.Send(It.Is<DeleteBasketCommand>(c => c.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_EmptyUserName_When_DeleteBasket_Then_ShouldCallMediator()
    {
        // Arrange
        var userName = string.Empty;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<DeleteBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteBasket(userName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _mockMediator.Verify(m => m.Send(It.Is<DeleteBasketCommand>(c => c.UserName == userName), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Checkout Tests

    [Fact]
    public async Task Given_ValidCheckoutDto_When_Checkout_Then_ShouldReturnOk()
    {
        // Arrange
        var checkoutDto = TestData.CreateBasketCheckoutDto();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CheckoutBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Checkout(checkoutDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        _mockMediator.Verify(m => m.Send(It.Is<CheckoutBasketCommand>(c => c.BasketCheckout == checkoutDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_CheckoutWithNonExistentBasket_When_Checkout_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var checkoutDto = TestData.CreateBasketCheckoutDto();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CheckoutBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Checkout(checkoutDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        _mockMediator.Verify(m => m.Send(It.Is<CheckoutBasketCommand>(c => c.BasketCheckout == checkoutDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_CheckoutWithEmptyBasket_When_Checkout_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var checkoutDto = TestData.CreateBasketCheckoutDto("emptyuser");

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CheckoutBasketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Checkout(checkoutDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Basket not found or empty", badRequestResult.Value?.ToString() ?? string.Empty);
        _mockMediator.Verify(m => m.Send(It.Is<CheckoutBasketCommand>(c => c.BasketCheckout == checkoutDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
