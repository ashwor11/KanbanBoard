using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class PersonRepository : EfRepositoryBase<KanbanDbContext, Person>, IPersonRepository
{
    public PersonRepository(KanbanDbContext context) : base(context)
    {
    }
}