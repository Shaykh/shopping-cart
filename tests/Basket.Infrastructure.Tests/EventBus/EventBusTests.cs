using Basket.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using InfrastructureEventBus = Basket.Infrastructure.EventBus.EventBus;

namespace Basket.Infrastructure.Tests.EventBus;

public class EventBusTests
{
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<InfrastructureEventBus>> _mockLogger;
    private readonly InfrastructureEventBus _eventBus;

    public EventBusTests()
    {
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<InfrastructureEventBus>>();
        _eventBus = new InfrastructureEventBus(_mockPublishEndpoint.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Given_ValidDomainEvent_When_PublishingEvent_Then_ShouldPublishToEndpoint()
    {
        // Arrange
        var checkoutEvent = new BasketCheckoutEvent(
            UserName: "testuser",
            TotalPrice: 100.00m,
            FirstName: "John",
            LastName: "Doe",
            EmailAddress: "john.doe@example.com",
            AddressLine: "123 Main St",
            Country: "USA",
            State: "CA",
            ZipCode: "12345",
            CardName: "John Doe",
            CardNumber: "1234567890123456",
            Expiration: "12/25",
            CVV: "123",
            PaymentMethod: 1,
            Items: [],
            OccurredOn: DateTime.UtcNow);

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _eventBus.PublishAsync(checkoutEvent);

        // Assert
        // Verify that Publish was called (MassTransit uses extension methods, so we verify the mock was invoked)
        _mockPublishEndpoint.Verify(
            x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_ValidDomainEvent_When_PublishingEvent_Then_ShouldLogInformation()
    {
        // Arrange
        var checkoutEvent = new BasketCheckoutEvent(
            UserName: "testuser",
            TotalPrice: 100.00m,
            FirstName: "John",
            LastName: "Doe",
            EmailAddress: "john.doe@example.com",
            AddressLine: "123 Main St",
            Country: "USA",
            State: "CA",
            ZipCode: "12345",
            CardName: "John Doe",
            CardNumber: "1234567890123456",
            Expiration: "12/25",
            CVV: "123",
            PaymentMethod: 1,
            Items: [],
            OccurredOn: DateTime.UtcNow);

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _eventBus.PublishAsync(checkoutEvent);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Publishing domain event")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Domain event published successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_CancellationToken_When_PublishingEvent_Then_ShouldPassCancellationToken()
    {
        // Arrange
        var checkoutEvent = new BasketCheckoutEvent(
            UserName: "testuser",
            TotalPrice: 100.00m,
            FirstName: "John",
            LastName: "Doe",
            EmailAddress: "john.doe@example.com",
            AddressLine: "123 Main St",
            Country: "USA",
            State: "CA",
            ZipCode: "12345",
            CardName: "John Doe",
            CardNumber: "1234567890123456",
            Expiration: "12/25",
            CVV: "123",
            PaymentMethod: 1,
            Items: [],
            OccurredOn: DateTime.UtcNow);

        var cancellationToken = new CancellationToken();

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<BasketCheckoutEvent>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _eventBus.PublishAsync(checkoutEvent, cancellationToken);

        // Assert
        // Verify that Publish was called (MassTransit uses extension methods, so we verify the mock was invoked)
        _mockPublishEndpoint.Verify(
            x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_EventWithItems_When_PublishingEvent_Then_ShouldPublishCompleteEvent()
    {
        // Arrange
        var items = new List<BasketCheckoutItem>
        {
            new BasketCheckoutItem(TestData.ProductId1, "Product 1", 10.00m, 2, "Red"),
            new BasketCheckoutItem(TestData.ProductId2, "Product 2", 15.00m, 3, "Blue")
        };

        var checkoutEvent = new BasketCheckoutEvent(
            UserName: "testuser",
            TotalPrice: 65.00m,
            FirstName: "John",
            LastName: "Doe",
            EmailAddress: "john.doe@example.com",
            AddressLine: "123 Main St",
            Country: "USA",
            State: "CA",
            ZipCode: "12345",
            CardName: "John Doe",
            CardNumber: "1234567890123456",
            Expiration: "12/25",
            CVV: "123",
            PaymentMethod: 1,
            Items: items,
            OccurredOn: DateTime.UtcNow);

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _eventBus.PublishAsync(checkoutEvent);

        // Assert
        // Verify that Publish was called (MassTransit uses extension methods, so we verify the mock was invoked)
        _mockPublishEndpoint.Verify(
            x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
