using AutoMapper;
using MediatR;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.ShoppingCartSessions.Commands.ShoppingCartSessionCommand;

public sealed record ShoppingCartSessionCommand : IRequest<ShoppingCartSessionDto>
{
    public List<string> Products { get; set; } = null!;
}  

public class ShoppingCartSessionHandler : IRequestHandler<ShoppingCartSessionCommand, ShoppingCartSessionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ShoppingCartSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShoppingCartSessionDto> Handle(ShoppingCartSessionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var newSession = await _unitOfWork.ShoppingCartSessionWriteRepository
            .AddAsync(new ShoppingCartSession(), cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var productId in request.Products)
        {
            var cartItem = new ShoppingCartItem()
            {
                ShoppingCartSessionId = newSession.Id,
                ProductId = productId
            };
            
            await _unitOfWork.ShoppingCartItemWriteRepository.AddAsync(cartItem, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync();
        return _mapper.Map<ShoppingCartSessionDto>(newSession);
    }
}