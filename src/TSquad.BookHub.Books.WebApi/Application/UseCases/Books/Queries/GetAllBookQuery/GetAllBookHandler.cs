using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;

namespace TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetAllBookQuery;

public sealed record GetAllBookQuery : IRequest<List<BookDto>>;

public class GetAllBookHandler : IRequestHandler<GetAllBookQuery, List<BookDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BookDto>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
    {
        var books = _unitOfWork.BookReadRepository.GetAllAsync();
        var response = books.ProjectTo<BookDto>(_mapper.ConfigurationProvider).ToList();
        return await Task.FromResult(response);
    }
}