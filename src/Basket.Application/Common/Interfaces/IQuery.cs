using MediatR;

namespace Basket.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
