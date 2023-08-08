using Core.Persistence.Repositories;
using Domain.Entities.Abstract;

namespace Domain.Entities.Concrete;

public class JobFeedback : Feedback
{
    public int JobId { get; set; }
    
}