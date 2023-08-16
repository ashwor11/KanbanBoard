namespace Application.Features.Boards.Dtos;

public class JobFeedbackToAddDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int JobId { get; set; }
    public string Content { get; set; }
}