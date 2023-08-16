using Domain.Entities.Abstract;

namespace Domain.Entities.Concrete;

public class CardFeedback : Feedback
{
    public int CardId { get; set; }


    public CardFeedback(int writtenByPersonId, string content) : base(writtenByPersonId, content)
    {
    }

    public CardFeedback()
    {
        
    }
}