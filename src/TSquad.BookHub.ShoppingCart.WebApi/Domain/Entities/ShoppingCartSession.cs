using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

public class ShoppingCartSession : CreationAuditable<long>
{
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
}