namespace Application.Features.Boards.Dtos;

public class CardFeedbackToAddDto 
{
    public int BoardId { get; set; }
    public int CardId { get; set; }
    public string Content { get; set; }

}