using Basket.Application.Common.Interfaces;
using Basket.Application.DTOs;

namespace Basket.Application.Commands;

public record CreateBasketCommand(ShoppingCartDto Basket) : ICommand<ShoppingCartDto>;
