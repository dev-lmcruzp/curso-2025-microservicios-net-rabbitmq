using FluentValidation;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.ShoppingCartSessions.Commands.ShoppingCartSessionCommand;

public class ShoppingCartSessionValidator : AbstractValidator<ShoppingCartSessionCommand>
{
    public ShoppingCartSessionValidator()
    {
        RuleFor(x=> x.Products).NotEmpty().NotNull();
    }
    
}