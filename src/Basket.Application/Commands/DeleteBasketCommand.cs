using Basket.Application.Common.Interfaces;

namespace Basket.Application.Commands;

public record DeleteBasketCommand(string UserName) : ICommand<bool>;
