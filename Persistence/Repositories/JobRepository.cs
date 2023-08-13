using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class JobRepository : EfRepositoryBase<KanbanDbContext, Job>, IJobRepository
{
    public JobRepository(KanbanDbContext context) : base(context)
    {
    }
}