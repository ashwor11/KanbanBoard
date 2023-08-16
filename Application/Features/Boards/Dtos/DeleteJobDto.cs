namespace Application.Features.Boards.Dtos;

public class DeleteJobDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int JobId { get; set; }
}