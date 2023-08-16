namespace Application.Features.Boards.Dtos;

public class AssignDueDateToCardDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public DateTime DueDate { get; set; }
}