namespace TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
}