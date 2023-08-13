using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class PersonBoardRepository : EfRepositoryBase<KanbanDbContext, PersonBoard>, IPersonBoardRepository
{
    public PersonBoardRepository(KanbanDbContext context) : base(context)
    {
    }
}