namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Dto.Remote;

public class RemoteBookDto
{
    public string ExternalBookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
    public string AuthorId { get; set; } = null!;
}