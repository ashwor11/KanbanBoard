using Core.Persistence.Repositories;
using Domain.Entities.Abstract;

namespace Domain.Entities.Concrete;

public class JobFeedback : Feedback
{
    public int JobId { get; set; }

    public JobFeedback(int writtenByPersonId, string content) : base(writtenByPersonId, content)
    {
    }

    public JobFeedback()
    {
        
    }

    public void UpdateFeedback( string content)
    {
        Content = content;
    }
}