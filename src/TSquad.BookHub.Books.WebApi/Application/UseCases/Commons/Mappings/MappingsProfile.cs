using AutoMapper;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.WebApi.Application.UseCases.Commons.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<CreateBookCommand, Book>();
        CreateMap<Book, BookDto>();
    }
}