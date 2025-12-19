using TSquad.BookHub.Authors.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.Authors.WebApi.Domain.Entities;

public class Author : BaseEntity<long>
{
    public string ExternalAuthorId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surnames { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<AcademicDegree> AcademicDegrees { get; set; } = new HashSet<AcademicDegree>();
}

// $ docker run --name postgres-18 -e POSTGRES_PASSWORD=Admin123. -p 5432:5432 -d postgres:postgres-18
// BookHubAuthors