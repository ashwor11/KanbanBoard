using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Application.Repositories;

public interface IOperationClaimRepository : IRepository<OperationClaim>, IAsyncRepository<OperationClaim>
{
    
}