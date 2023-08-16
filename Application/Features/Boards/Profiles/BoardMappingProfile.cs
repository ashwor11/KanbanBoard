using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Models;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap<Board, DeletedBoardDto>();
        CreateMap<Board, GetWholeBoardDto>().ForMember(x => x.Cards, opt => opt.MapFrom(x => x.Cards))
            .ForMember(x => x.Persons, opt => opt.MapFrom(x => x.PersonBoards.Select(x => x.Person)));
        CreateMap<Card, GetCardDto>().ForMember(x => x.Feedbacks, opt => opt.MapFrom(x => x.Feedbacks))
            .ForMember(x => x.Jobs, opt => opt.MapFrom(x => x.Jobs));

        CreateMap<JobFeedback, GetJobFeedbackDto>();
        CreateMap<Job, GetJobDto>().ForMember(x => x.Feedbacks, opt => opt.MapFrom(x => x.Feedbacks));
        CreateMap<CardFeedback, GetCardFeedbackDto>();
        CreateMap<Person, GetPersonsForBoardDto>();

        CreateMap<Board, GetPersonsBoardDto>().ForMember(x=>x.BoardId, opt=>opt.MapFrom(x=>x.Id)).ForMember(x=>x.BoardName, opt=>opt.MapFrom(x=>x.Name));
        CreateMap<List<Board>, GetPersonsAllBoardsModel>().ForMember(x=>x.Boards, opt=> opt.MapFrom(x=>x));
    }
}