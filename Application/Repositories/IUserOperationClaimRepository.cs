using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Application.Repositories;

public interface IUserOperationClaimRepository : IRepository<UserOperationClaim>, IAsyncRepository<UserOperationClaim>
{
    
}