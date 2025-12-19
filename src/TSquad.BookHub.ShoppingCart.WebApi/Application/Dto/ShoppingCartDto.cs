namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;

public class ShoppingCartDto
{
    public long ShoppingCartId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ShoppingCartItemDto> Items { get; set; } = null!;
}