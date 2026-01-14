using Basket.Application.Commands;
using Basket.Application.Handlers.Commands;
using Basket.Application.Repositories;
using Moq;

namespace Basket.Application.Tests.Handlers.Commands;

public class DeleteBasketCommandHandlerTests
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly DeleteBasketCommandHandler _handler;

    public DeleteBasketCommandHandlerTests()
    {
        _mockRepository = new Mock<IBasketRepository>();
        _handler = new DeleteBasketCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Given_ExistingBasket_When_HandlingCommand_Then_ShouldDeleteBasketAndReturnTrue()
    {
        // Arrange
        var userName = "testuser";
        _mockRepository.Setup(x => x.DeleteBasketAsync(userName))
            .ReturnsAsync(true);

        var command = new DeleteBasketCommand(userName);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(x => x.DeleteBasketAsync(userName), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentBasket_When_HandlingCommand_Then_ShouldReturnFalse()
    {
        // Arrange
        var userName = "nonexistent";
        _mockRepository.Setup(x => x.DeleteBasketAsync(userName))
            .ReturnsAsync(false);

        var command = new DeleteBasketCommand(userName);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(x => x.DeleteBasketAsync(userName), Times.Once);
    }
}
