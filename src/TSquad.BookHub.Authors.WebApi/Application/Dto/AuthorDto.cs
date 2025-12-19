namespace TSquad.BookHub.Authors.WebApi.Application.Dto;

public class AuthorDto
{
    public string ExternalAuthorId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surnames { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
}