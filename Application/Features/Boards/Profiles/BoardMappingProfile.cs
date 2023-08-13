using Application.Features.Boards.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap<Board, DeletedBoardDto>();
    }
}