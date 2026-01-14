using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using Basket.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

/// <summary>
/// Controller for managing shopping baskets
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Basket")]
public class BasketController(IMediator mediator, ILogger<BasketController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<BasketController> _logger = logger;

    /// <summary>
    /// Retrieves a shopping basket by user name
    /// </summary>
    /// <param name="userName">The user name to retrieve the basket for</param>
    /// <returns>The shopping basket if found</returns>
    /// <response code="200">Returns the shopping basket</response>
    /// <response code="404">Basket not found for the specified user</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
    {
        _logger.LogInformation("Getting basket for user: {UserName}", userName);

        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);

        if (basket == null)
        {
            _logger.LogWarning("Basket not found for user: {UserName}", userName);
            return NotFound($"Basket not found for user: {userName}");
        }

        return Ok(basket);
    }

    /// <summary>
    /// Creates or updates a shopping basket
    /// </summary>
    /// <param name="basket">The shopping basket data to create or update</param>
    /// <returns>The created or updated shopping basket</returns>
    /// <response code="200">Basket created or updated successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    /// <response code="500">Internal server error</response>
    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCartDto>> UpdateBasket([FromBody] ShoppingCartDto basket)
    {
        _logger.LogInformation("Updating basket for user: {UserName}", basket.UserName);

        var command = new CreateBasketCommand(basket);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a shopping basket by user name
    /// </summary>
    /// <param name="userName">The user name whose basket should be deleted</param>
    /// <returns>Success message if the basket was deleted</returns>
    /// <response code="200">Basket deleted successfully</response>
    /// <response code="404">Basket not found for the specified user</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteBasket(string userName)
    {
        _logger.LogInformation("Deleting basket for user: {UserName}", userName);

        var command = new DeleteBasketCommand(userName);
        var result = await _mediator.Send(command);

        if (!result)
        {
            _logger.LogWarning("Basket not found for deletion: {UserName}", userName);
            return NotFound($"Basket not found for user: {userName}");
        }

        return Ok(new { message = $"Basket for user {userName} deleted successfully" });
    }

    /// <summary>
    /// Processes the checkout for a shopping basket
    /// </summary>
    /// <param name="basketCheckout">The checkout information including payment and shipping details</param>
    /// <returns>Success message if checkout was processed</returns>
    /// <response code="200">Checkout processed successfully. The basket checkout event has been published.</response>
    /// <response code="400">Invalid request data, validation errors, or basket is empty</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("checkout", Name = "CheckoutBasket")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Checkout([FromBody] BasketCheckoutDto basketCheckout)
    {
        _logger.LogInformation("Processing checkout for user: {UserName}", basketCheckout.UserName);

        var command = new CheckoutBasketCommand(basketCheckout);
        var result = await _mediator.Send(command);

        if (!result)
        {
            _logger.LogWarning("Checkout failed for user: {UserName} - Basket not found or empty", basketCheckout.UserName);
            return BadRequest("Basket not found or empty. Cannot proceed with checkout.");
        }

        return Ok(new { message = "Checkout processed successfully" });
    }
}
