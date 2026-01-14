using Basket.Application.DTOs;
using Basket.Application.Validators;
using FluentValidation.TestHelper;

namespace Basket.Application.Tests.Validators;

public class CartItemDtoValidatorTests
{
    private readonly CartItemDtoValidator _validator;

    public CartItemDtoValidatorTests()
    {
        _validator = new CartItemDtoValidator();
    }

    [Fact]
    public void Given_ValidCartItem_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product 1",
            Price = 10.00m,
            Quantity = 2
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Given_EmptyProductId_When_Validating_Then_ShouldFail()
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.Empty,
            ProductName = "Product 1",
            Price = 10.00m,
            Quantity = 2
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Given_InvalidProductName_When_Validating_Then_ShouldFail(string? productName)
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = productName!,
            Price = 10.00m,
            Quantity = 2
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.50)]
    public void Given_NegativePrice_When_Validating_Then_ShouldFail(decimal price)
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product 1",
            Price = price,
            Quantity = 2
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Given_ZeroPrice_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product 1",
            Price = 0m,
            Quantity = 2
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_InvalidQuantity_When_Validating_Then_ShouldFail(int quantity)
    {
        // Arrange
        var item = new CartItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product 1",
            Price = 10.00m,
            Quantity = quantity
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }
}
