using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using TSquad.BookHub.Authors.WebApi.Application.Dto;
using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;

namespace TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Queries.GetAllAuthorQuery;


public sealed record GetAllAuthorQuery : IRequest<List<AuthorDto>>;

public class GetAllAuthorHandler : IRequestHandler<GetAllAuthorQuery, List<AuthorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetAllAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<List<AuthorDto>> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
    {
        var authors = _unitOfWork.AuthorReadRepository.GetAllAsync();
        var response = authors.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider).ToList(); 
        return await Task.FromResult(response);
    }
}