namespace TSquad.BookHub.Books.WebApi.Application.Dto;

public class BookDto
{
    public string ExternalBookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
    public string AuthorId { get; set; } = null!;
}