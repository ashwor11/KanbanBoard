namespace Application.Features.Boards.Dtos;

public class ChangeCardStatusDto
{
    public int BoardId { get; set; }
    public int CardId{ get; set; }
    public string Status { get; set; }
}