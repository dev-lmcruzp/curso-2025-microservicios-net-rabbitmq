using AutoMapper;
using MediatR;
using TSquad.BookHub.Authors.WebApi.Application.Dto;
using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Commands.CreateAuthorCommand;

public sealed record CreateAuthorCommand : IRequest<AuthorDto>
{
    public string Name { get; set; } = null!;
    public string Surnames { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
}

public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var newAuthor = new Author()
        {
            Name = request.Name,
            Surnames = request.Surnames,
            DateOfBirth = request.DateOfBirth,
            ExternalAuthorId = Guid.NewGuid().ToString()
        };
        await _unitOfWork.AuthorWriteRepository.AddAsync(newAuthor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<AuthorDto>(newAuthor);
    }
}