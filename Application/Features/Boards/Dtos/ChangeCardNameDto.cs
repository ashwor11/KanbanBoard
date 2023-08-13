namespace Application.Features.Boards.Dtos;

public class ChangeCardNameDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public string Name { get; set; }
}