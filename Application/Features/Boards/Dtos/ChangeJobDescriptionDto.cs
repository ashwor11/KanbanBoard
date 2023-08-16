namespace Application.Features.Boards.Dtos;

public class ChangeJobDescriptionDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int JobId { get; set; }
    public string JobDescription { get; set; }
}