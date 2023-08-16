namespace Application.Features.Boards.Dtos;

public class AddedJobFeedbackDto
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int WrittenByPersonId { get; set; }
    public string Content { get; set; }
}