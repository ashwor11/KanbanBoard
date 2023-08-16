namespace Application.Features.Boards.Dtos;

public class AssignCardToPersonDto
{
    public int PersonId { get; set; }
    public int BoardId { get; set; }
    public int CardId { get; set; }
}