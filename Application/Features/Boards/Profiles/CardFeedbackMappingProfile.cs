using Application.Features.Boards.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class CardFeedbackMappingProfile : Profile
{
    public CardFeedbackMappingProfile()
    {
        CreateMap<CardFeedback, AddedCardFeedbackDto>();
    }
}