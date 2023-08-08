using Core.Persistence.Repositories;

namespace Domain.Entities.Concrete;

public class Job : Entity
{
    public int CardId { get; set; }
    public bool IsDone { get; set; }

    public virtual HashSet<JobFeedback> Feedbacks {get; set; }

    public Job()
    {
        Feedbacks = new HashSet<JobFeedback>();
    }
}