using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class BasketController(IMediator mediator, ILogger<BasketController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<BasketController> _logger = logger;

    /// <summary>
    /// Récupère le panier d'un utilisateur
    /// </summary>
    /// <param name="userName">Nom d'utilisateur</param>
    /// <returns>Le panier de l'utilisateur</returns>
    /// <response code="200">Panier trouvé</response>
    /// <response code="404">Panier non trouvé</response>
    [HttpGet("{userName}")]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Crée ou met à jour un panier
    /// </summary>
    /// <param name="basket">Données du panier</param>
    /// <returns>Le panier créé ou mis à jour</returns>
    /// <response code="200">Panier créé ou mis à jour avec succès</response>
    /// <response code="400">Données invalides</response>
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ShoppingCartDto>> UpdateBasket([FromBody] ShoppingCartDto basket)
    {
        _logger.LogInformation("Updating basket for user: {UserName}", basket.UserName);

        var command = new CreateBasketCommand(basket);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Supprime un panier
    /// </summary>
    /// <param name="userName">Nom d'utilisateur</param>
    /// <returns>Résultat de la suppression</returns>
    /// <response code="200">Panier supprimé avec succès</response>
    /// <response code="404">Panier non trouvé</response>
    [HttpDelete("{userName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Valide le panier et déclenche la commande
    /// </summary>
    /// <param name="basketCheckout">Données de checkout</param>
    /// <returns>Résultat du checkout</returns>
    /// <response code="200">Checkout réussi</response>
    /// <response code="400">Données invalides ou panier vide</response>
    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
