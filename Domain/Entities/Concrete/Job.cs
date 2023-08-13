using Core.Persistence.Repositories;

namespace Domain.Entities.Concrete;

public class Job : Entity
{
    public int CardId { get; set; }
    public string JobDescription { get; set; }
    public bool IsDone { get; set; }
    

    public virtual List<JobFeedback> Feedbacks {get; set; }

    public Job()
    {
        Feedbacks = new List<JobFeedback>();
    }

    public void AddFeedback()
    {
        Feedbacks.Add(new JobFeedback());
    }

    public void UpdateFeedback(int feedbackId, string content)
    {
        Feedbacks.First(x => x.Id == feedbackId).Content = content;
    }
    public void DeleteFeedback(int feedbackId)
    {
        JobFeedback feedback = Feedbacks.First(x => x.Id == feedbackId);
        feedback = null;
    }
}