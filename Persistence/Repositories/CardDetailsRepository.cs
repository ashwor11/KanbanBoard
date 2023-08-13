using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CardDetailsRepository : EfRepositoryBase<KanbanDbContext, CardDetails>, ICardDetailsRepository

{
    public CardDetailsRepository(KanbanDbContext context) : base(context)
    {
    }
}