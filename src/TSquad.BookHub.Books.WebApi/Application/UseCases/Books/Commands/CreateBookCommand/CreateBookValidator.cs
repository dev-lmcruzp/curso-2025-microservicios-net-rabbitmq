using FluentValidation;

namespace TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;

public class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.AuthorId).NotNull().NotEmpty();
    }
}