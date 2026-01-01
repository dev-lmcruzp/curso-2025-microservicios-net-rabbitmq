namespace TSquad.BookHub.Api.Gateway.Application.RemoteDto;

public class BookRemoteDto
{
    public string ExternalBookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
    public string AuthorId { get; set; } = null!;
    public AuthorRemoteDto? Author { get; set; } = null!;
}