using Basket.Application.DTOs;
using Basket.Application.Validators;
using FluentValidation.TestHelper;

namespace Basket.Application.Tests.Validators;

public class ShoppingCartDtoValidatorTests
{
    private readonly ShoppingCartDtoValidator _validator;

    public ShoppingCartDtoValidatorTests()
    {
        _validator = new ShoppingCartDtoValidator();
    }

    [Fact]
    public void Given_ValidShoppingCart_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var cart = new ShoppingCartDto
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
        };

        // Act
        var result = _validator.TestValidate(cart);

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
        var cart = new ShoppingCartDto
        {
            UserName = userName!,
            Items = []
        };

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Given_EmptyItemsList_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var cart = new ShoppingCartDto
        {
            UserName = "testuser",
            Items = []
        };

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Given_InvalidItemInList_When_Validating_Then_ShouldFail()
    {
        // Arrange
        var cart = new ShoppingCartDto
        {
            UserName = "testuser",
            Items =
            [
                new CartItemDto
                {
                    ProductId = Guid.Empty, // Invalid
                    ProductName = "Product 1",
                    Price = 10.00m,
                    Quantity = 2
                }
            ]
        };

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
    }
}
