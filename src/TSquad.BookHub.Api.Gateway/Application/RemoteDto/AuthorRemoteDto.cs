namespace TSquad.BookHub.Api.Gateway.Application.RemoteDto;

public class AuthorRemoteDto
{
    public string ExternalAuthorId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surnames { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
}