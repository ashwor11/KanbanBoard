using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface IJobFeedbackRepository : IRepository<JobFeedback>, IAsyncRepository<JobFeedback>
{
    
}