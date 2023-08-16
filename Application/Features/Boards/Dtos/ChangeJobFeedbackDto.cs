namespace Application.Features.Boards.Dtos;

public class ChangeJobFeedbackDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int JobId { get; set; }
    public int JobFeedbackId { get; set; }
    public string Content { get; set; }
}