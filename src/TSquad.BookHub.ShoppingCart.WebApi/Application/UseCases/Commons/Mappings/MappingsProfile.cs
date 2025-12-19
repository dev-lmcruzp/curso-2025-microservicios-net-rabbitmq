using AutoMapper;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.Commons.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        // CreateMap<ShoppingCartSessionCommand, ShoppingCartSession>();
        CreateMap<ShoppingCartSession, ShoppingCartSessionDto>();
    }
}