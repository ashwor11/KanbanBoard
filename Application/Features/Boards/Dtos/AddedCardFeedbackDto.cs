namespace Application.Features.Boards.Dtos;

public class AddedCardFeedbackDto
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public int WrittenByPersonId { get; set; }
    public string Content { get; set; }
}