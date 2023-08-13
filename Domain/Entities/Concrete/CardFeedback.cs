using Domain.Entities.Abstract;

namespace Domain.Entities.Concrete;

public class CardFeedback : Feedback
{
    public int CardId { get; set; }


    public CardFeedback(string content) : base(content)
    {
    }

    public CardFeedback()
    {
        
    }
}