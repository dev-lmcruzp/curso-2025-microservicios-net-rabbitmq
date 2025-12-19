namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;

public class ShoppingCartItemDto
{
    public string BookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
}