using Basket.Application.DTOs;
using Basket.Application.Validators;
using FluentValidation.TestHelper;

namespace Basket.Application.Tests.Validators;

public class BasketCheckoutDtoValidatorTests
{
    private readonly BasketCheckoutDtoValidator _validator;

    public BasketCheckoutDtoValidatorTests()
    {
        _validator = new BasketCheckoutDtoValidator();
    }

    [Fact]
    public void Given_ValidCheckoutDto_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var checkout = new BasketCheckoutDto
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
        };

        // Act
        var result = _validator.TestValidate(checkout);

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
        var checkout = CreateValidCheckout();
        checkout.UserName = userName!;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("notanemail")]
    public void Given_InvalidEmail_When_Validating_Then_ShouldFail(string? email)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.EmailAddress = email!;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Given_InvalidFirstName_When_Validating_Then_ShouldFail(string? firstName)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.FirstName = firstName!;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Given_InvalidLastName_When_Validating_Then_ShouldFail(string? lastName)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.LastName = lastName!;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Theory]
    [InlineData("123456789012")] // Too short
    [InlineData("12345678901234567890")] // Too long
    public void Given_InvalidCardNumber_When_Validating_Then_ShouldFail(string cardNumber)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.CardNumber = cardNumber;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }

    [Theory]
    [InlineData("13/25")] // Invalid month
    [InlineData("00/25")] // Invalid month
    [InlineData("12/2")] // Invalid format - missing leading zero
    [InlineData("1/25")] // Invalid format - single digit month
    public void Given_InvalidExpiration_When_Validating_Then_ShouldFail(string expiration)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.Expiration = expiration;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Expiration);
    }

    [Theory]
    [InlineData("12")] // Too short
    [InlineData("12345")] // Too long
    public void Given_InvalidCVV_When_Validating_Then_ShouldFail(string cvv)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.CVV = cvv;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CVV);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_InvalidTotalPrice_When_Validating_Then_ShouldFail(decimal totalPrice)
    {
        // Arrange
        var checkout = CreateValidCheckout();
        checkout.TotalPrice = totalPrice;

        // Act
        var result = _validator.TestValidate(checkout);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    private static BasketCheckoutDto CreateValidCheckout()
    {
        return new BasketCheckoutDto
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
        };
    }
}
