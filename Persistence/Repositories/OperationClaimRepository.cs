using Application.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OperationClaimRepository : EfRepositoryBase<KanbanDbContext, OperationClaim>, IOperationClaimRepository
{
    public OperationClaimRepository(KanbanDbContext context) : base(context)
    {
    }
}