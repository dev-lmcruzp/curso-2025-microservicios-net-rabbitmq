using TSquad.BookHub.Books.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.Books.WebApi.Domain.Entities;

public class Book : BaseEntity<long>
{
    public string ExternalBookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
    public string AuthorId { get; set; } = null!;
}