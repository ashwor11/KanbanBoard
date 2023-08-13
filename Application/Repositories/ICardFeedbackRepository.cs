using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface ICardFeedbackRepository : IRepository<CardFeedback>, IAsyncRepository<CardFeedback>
{
    
}