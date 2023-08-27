namespace Application.Features.Boards.Dtos;

public class ChangeCardColorDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public string ColorInHex { get; set; }

}