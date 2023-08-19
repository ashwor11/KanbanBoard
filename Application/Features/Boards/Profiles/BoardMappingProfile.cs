using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Models;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Entities.Enums;

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
        CreateMap<Job, GetJobDto>().ForMember(x => x.Feedbacks, opt => opt.MapFrom(x => x.Feedbacks)).ForMember(x=>x.Description, opt => opt.MapFrom(x=>x.JobDescription));
        CreateMap<CardFeedback, GetCardFeedbackDto>();
        CreateMap<Person, GetPersonsForBoardDto>();

        CreateMap<Board, GetPersonsBoardDto>();
        CreateMap<List<Board>, GetPersonsAllBoardsModel>().ForMember(x=>x.Boards, opt=> opt.MapFrom(x=>x));

        CreateMap<Board, GetBoardByIdDto>()
            .ForMember(x => x.Persons, opt => opt.MapFrom(x => x.PersonBoards.Select(x => x.Person))).
            ForMember(x=>x.CreatorId , cfg => cfg.MapFrom(x=>x.CreatorUserId)).
        ConvertUsing((board, dto, ctx) =>
            {
                var cards = board.Cards;

                List<Card> backlogCards = cards.Where(card => card.Status == CardStatus.Backlog).ToList();
                List<Card> toDoCards = cards.Where(card => card.Status == CardStatus.ToDo).ToList();
                List<Card> inProgressCards = cards.Where(card => card.Status == CardStatus.InProgress).ToList();
                List<Card> reviewCards = cards.Where(card => card.Status == CardStatus.Review).ToList();
                List<Card> doneCards = cards.Where(card => card.Status == CardStatus.Done).ToList();

                Column backlog = new() { Id = "Backlog", Cards = ctx.Mapper.Map<List<GetCardDto>>(backlogCards) };
                Column toDo = new() { Id = "ToDo", Cards = ctx.Mapper.Map<List<GetCardDto>>(toDoCards) };
                Column inProgress = new() { Id = "InProgress", Cards = ctx.Mapper.Map<List<GetCardDto>>(inProgressCards) };
                Column review = new() { Id = "Review", Cards = ctx.Mapper.Map<List<GetCardDto>>(reviewCards) };
                Column done = new() { Id = "Done", Cards = ctx.Mapper.Map<List<GetCardDto>>(doneCards) };






                List<GetPersonsForBoardDto> persons = ctx.Mapper.Map<List<GetPersonsForBoardDto>>(board.PersonBoards.Select(x=>x.Person));
            dto = new()
            {
                Id = board.Id,
               Backlog = backlog,
               InProgress = inProgress,
               CreatorId = board.CreatorUserId,
                Description = board.Description,
                Name = board.Name,
                Persons = persons,
                Review = review,
                Done = done,
                ToDo = toDo
            };
            return dto;


        });



    }

    




}