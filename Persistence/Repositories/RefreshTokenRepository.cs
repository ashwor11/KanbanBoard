using Application.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<KanbanDbContext,RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(KanbanDbContext context) : base(context)
    {
    }
}