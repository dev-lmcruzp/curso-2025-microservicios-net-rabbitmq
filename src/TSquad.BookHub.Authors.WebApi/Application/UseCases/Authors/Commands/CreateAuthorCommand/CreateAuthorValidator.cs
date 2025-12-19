using FluentValidation;

namespace TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorValidator()
    {
        RuleFor(p => p.Name).NotEmpty().NotNull();
        RuleFor(p => p.Surnames).NotEmpty().NotNull();
    }
}