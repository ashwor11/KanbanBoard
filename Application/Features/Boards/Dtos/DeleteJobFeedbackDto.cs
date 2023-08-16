namespace Application.Features.Boards.Dtos;

public class DeleteJobFeedbackDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int JobId { get; set; }
    public int JobFeedbackId { get; set; }
}