using AutoMapper;
using TSquad.BookHub.Authors.WebApi.Application.Dto;
using TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Commands.CreateAuthorCommand;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Application.UseCases.Commons.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<CreateAuthorCommand, Author>();
        CreateMap<Author, AuthorDto>();
    }
}