using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Validators;
using FluentValidation.TestHelper;

namespace Basket.Application.Tests.Validators;

public class CreateBasketCommandValidatorTests
{
    private readonly CreateBasketCommandValidator _validator;

    public CreateBasketCommandValidatorTests()
    {
        _validator = new CreateBasketCommandValidator();
    }

    [Fact]
    public void Given_ValidCommand_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var command = new CreateBasketCommand(new ShoppingCartDto
        {
            UserName = "testuser",
            Items =
            [
                new CartItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product 1",
                    Price = 10.00m,
                    Quantity = 2
                }
            ]
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Given_NullBasket_When_Validating_Then_ShouldFail()
    {
        // Arrange
        var command = new CreateBasketCommand(null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Basket);
    }
}

public class UpdateBasketCommandValidatorTests
{
    private readonly UpdateBasketCommandValidator _validator;

    public UpdateBasketCommandValidatorTests()
    {
        _validator = new UpdateBasketCommandValidator();
    }

    [Fact]
    public void Given_ValidCommand_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var command = new UpdateBasketCommand(new ShoppingCartDto
        {
            UserName = "testuser",
            Items = []
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Given_NullBasket_When_Validating_Then_ShouldFail()
    {
        // Arrange
        var command = new UpdateBasketCommand(null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Basket);
    }
}

public class DeleteBasketCommandValidatorTests
{
    private readonly DeleteBasketCommandValidator _validator;

    public DeleteBasketCommandValidatorTests()
    {
        _validator = new DeleteBasketCommandValidator();
    }

    [Fact]
    public void Given_ValidUserName_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var command = new DeleteBasketCommand("testuser");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_InvalidUserName_When_Validating_Then_ShouldFail(string? userName)
    {
        // Arrange
        var command = new DeleteBasketCommand(userName!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }
}

public class CheckoutBasketCommandValidatorTests
{
    private readonly CheckoutBasketCommandValidator _validator;

    public CheckoutBasketCommandValidatorTests()
    {
        _validator = new CheckoutBasketCommandValidator();
    }

    [Fact]
    public void Given_ValidCommand_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var command = new CheckoutBasketCommand(new BasketCheckoutDto
        {
            UserName = "testuser",
            TotalPrice = 100.00m,
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
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Given_NullBasketCheckout_When_Validating_Then_ShouldFail()
    {
        // Arrange
        var command = new CheckoutBasketCommand(null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BasketCheckout);
    }
}
