using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CardRepository : EfRepositoryBase<KanbanDbContext, Card>, ICardRepository
{
    public CardRepository(KanbanDbContext context) : base(context)
    {
    }
}