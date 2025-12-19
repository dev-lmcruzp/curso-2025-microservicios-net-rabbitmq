using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

public class ShoppingCartItem : CreationAuditable<long>
{
    public long ShoppingCartSessionId { get; set; }
    public string ProductId { get; set; } = null!;
    public ShoppingCartSession ShoppingCartSession { get; set; } = null!;
}