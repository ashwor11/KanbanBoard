using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CardFeedbackRepository : EfRepositoryBase<KanbanDbContext, CardFeedback>, ICardFeedbackRepository
{
    public CardFeedbackRepository(KanbanDbContext context) : base(context)
    {
    }
}