using AutoMapper;
using MediatR;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Books.WebApi.Domain.Entities;
using TSquad.BookHub.RabbitMQ.Bus.EventBus;
using TSquad.BookHub.RabbitMQ.Bus.EventQueue;

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
    private readonly IEventBus _eventBus;

    public CreateBookHandler(IUnitOfWork unitOfWork, IMapper mapper, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var newBook = _mapper.Map<Book>(request);
        newBook.ExternalBookId = Guid.NewGuid().ToString();
        
        newBook = await _unitOfWork.BookWriteRepository
            .AddAsync(newBook, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _eventBus.Publish(new EmailEventQueue("lcruz@yopmail.com",
            request.Title,
            "Este es un contenido de ejemplo"));
        return _mapper.Map<BookDto>(newBook);
    }
}