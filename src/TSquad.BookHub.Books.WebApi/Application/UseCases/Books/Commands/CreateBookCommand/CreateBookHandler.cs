using AutoMapper;
using MediatR;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;

public sealed record CreateBookCommand : IRequest<BookDto>
{
    public string Title { get; set; } = null!;
    public DateOnly? DatePublic { get; set; }
    public string AuthorId { get; set; } = null!;
}  

public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var newBook = _mapper.Map<Book>(request);
        newBook.ExternalBookId = Guid.NewGuid().ToString();
        
        newBook = await _unitOfWork.BookWriteRepository
            .AddAsync(newBook, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BookDto>(newBook);
    }
}