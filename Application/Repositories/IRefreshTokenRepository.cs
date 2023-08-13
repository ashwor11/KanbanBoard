using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Application.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IAsyncRepository<RefreshToken>
{
    
}