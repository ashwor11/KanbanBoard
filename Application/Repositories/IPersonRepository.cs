using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface IPersonRepository : IRepository<Person>, IAsyncRepository<Person>
{
    
}