namespace Application.Features.Boards.Dtos;

public class ChangeCardFeedbackDto
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public int CardFeedbackId { get; set; }
    public string Content { get; set; }
}