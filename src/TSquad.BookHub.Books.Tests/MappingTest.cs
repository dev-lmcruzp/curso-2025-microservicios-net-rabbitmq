using AutoMapper;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.Tests;

public class MappingTest : Profile
{
    public MappingTest() {
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookCommand, Book>();
    }

}