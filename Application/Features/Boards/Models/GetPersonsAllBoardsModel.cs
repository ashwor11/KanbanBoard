using Application.Features.Boards.Dtos;

namespace Application.Features.Boards.Models;

public class GetPersonsAllBoardsModel
{
    public List<GetPersonsBoardDto> Boards { get; set; }
}