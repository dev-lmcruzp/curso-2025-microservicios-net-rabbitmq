namespace TSquad.BookHub.Books.WebApi.Domain.Entities.Base;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
}