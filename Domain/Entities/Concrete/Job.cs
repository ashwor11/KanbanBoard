using Core.Persistence.Repositories;

namespace Domain.Entities.Concrete;

public class Job : Entity
{
    public int CardId { get; set; }
    public string? JobDescription { get; set; }
    public bool IsDone { get; set; }
    

    public virtual List<JobFeedback> Feedbacks {get; set; }

    public Job()
    {
        Feedbacks = new List<JobFeedback>();
    }

    public void ChangeDescriptionOfJob(int jobId, string description)
    {
        JobDescription = description;
    }
    public void MarkJobAsDone(int jobId, string description)
    {
        IsDone = true;
    }
    public void MarkJobAsUnDone(int jobId, string description)
    {
        IsDone = false;
    }

    public void AddFeedback(int writtenByPersonId, string content)
    {
        Feedbacks.Add(new JobFeedback(writtenByPersonId,content));
    }
    public void DeleteFeedback(int feedbackId)
    {
        JobFeedback feedback = Feedbacks.First(x => x.Id == feedbackId);
        feedback = null;
    }
}