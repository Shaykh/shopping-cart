using Basket.Application.Common.Interfaces;
using Basket.Application.DTOs;

namespace Basket.Application.Queries;

public record GetBasketByUserNameQuery(string UserName) : IQuery<ShoppingCartDto?>;
