using AutoMapper;
using MediatR;
using TSquad.BookHub.Authors.WebApi.Application.Dto;
using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;

namespace TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Queries.GetAuthorQuery;

public sealed record GetAuthorQuery(string ExternalAuthorId) : IRequest<AuthorDto?>;

public class GetAuthorHandler : IRequestHandler<GetAuthorQuery, AuthorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
    {
        var currentAuthor = await _unitOfWork.AuthorReadRepository
            .FindAsync(x => x.ExternalAuthorId.Equals(request.ExternalAuthorId), cancellationToken);

        return currentAuthor is null 
            ? null 
            : _mapper.Map<AuthorDto>(currentAuthor);
    }
}