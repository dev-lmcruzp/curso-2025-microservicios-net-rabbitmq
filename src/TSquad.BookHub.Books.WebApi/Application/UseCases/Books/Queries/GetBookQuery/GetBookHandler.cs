using AutoMapper;
using MediatR;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;

namespace TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetBookQuery;

public sealed record GetBookQuery(string id) : IRequest<BookDto?>;

public class GetBookHandler : IRequestHandler<GetBookQuery, BookDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var currentBook = await _unitOfWork.BookReadRepository
            .FindAsync(x => x.ExternalBookId.Equals(request.id), cancellationToken);
        return currentBook is null
            ? null
            : _mapper.Map<BookDto>(currentBook);
    }
}