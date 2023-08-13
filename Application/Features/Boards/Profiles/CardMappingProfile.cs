using Application.Features.Boards.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class CardMappingProfile : Profile
{
    public CardMappingProfile()
    {
        CreateMap<Card, AddedCardDto>();
    }
}