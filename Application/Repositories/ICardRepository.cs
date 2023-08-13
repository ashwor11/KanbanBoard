using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface ICardRepository : IRepository<Card>, IAsyncRepository<Card>
{
    
}