using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface ICardDetailsRepository : IRepository<CardDetails>, IAsyncRepository<CardDetails>
{
    
}