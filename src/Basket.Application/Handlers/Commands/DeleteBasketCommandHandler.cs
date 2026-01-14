using Basket.Application.Commands;
using Basket.Application.Common.Interfaces;
using Basket.Application.Repositories;

namespace Basket.Application.Handlers.Commands;

public class DeleteBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        return await _basketRepository.DeleteBasketAsync(request.UserName);
    }
}
