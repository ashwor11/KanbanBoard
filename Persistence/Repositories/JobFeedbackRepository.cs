using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class JobFeedbackRepository : EfRepositoryBase<KanbanDbContext, JobFeedback>, IJobFeedbackRepository
{
    public JobFeedbackRepository(KanbanDbContext context) : base(context)
    {
    }
}