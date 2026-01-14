using Basket.Application.Common.Interfaces;
using Basket.Application.DTOs;

namespace Basket.Application.Commands;

public record UpdateBasketCommand(ShoppingCartDto Basket) : ICommand<ShoppingCartDto>;
