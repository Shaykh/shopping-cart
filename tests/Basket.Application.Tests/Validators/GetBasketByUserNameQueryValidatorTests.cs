using Basket.Application.Queries;
using Basket.Application.Validators;
using FluentValidation.TestHelper;

namespace Basket.Application.Tests.Validators;

public class GetBasketByUserNameQueryValidatorTests
{
    private readonly GetBasketByUserNameQueryValidator _validator;

    public GetBasketByUserNameQueryValidatorTests()
    {
        _validator = new GetBasketByUserNameQueryValidator();
    }

    [Fact]
    public void Given_ValidUserName_When_Validating_Then_ShouldPass()
    {
        // Arrange
        var query = new GetBasketByUserNameQuery("testuser");

        // Act
        var result = _validator.TestValidate(query);

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
        var query = new GetBasketByUserNameQuery(userName!);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }
}
