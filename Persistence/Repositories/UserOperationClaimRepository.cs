using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<KanbanDbContext, UserOperationClaim>, Application.Repositories.IUserOperationClaimRepository
{
    public UserOperationClaimRepository(KanbanDbContext context) : base(context)
    {
    }
}