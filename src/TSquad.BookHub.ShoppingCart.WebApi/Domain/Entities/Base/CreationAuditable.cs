namespace TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;

public interface ICreationAuditable
{
    DateTime CreatedAt { get; set; }
    // string CreatedBy { get; set; }
}

public class CreationAuditable<TId> : BaseEntity<TId>, ICreationAuditable
{
    public DateTime CreatedAt { get; set; }
    // public string CreatedBy { get; set; } = null!;
}