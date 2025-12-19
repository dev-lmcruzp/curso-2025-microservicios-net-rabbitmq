using TSquad.BookHub.Authors.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.Authors.WebApi.Domain.Entities;

public class AcademicDegree : BaseEntity<long>
{
    public long AuthorId { get; set; }
    public string ExternalAcademicDegreeId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AcademicInstitution { get; set; } = null!;
    public DateOnly? GraduationDate { get; set; }
    public Author Author { get; set; } = null!;
}