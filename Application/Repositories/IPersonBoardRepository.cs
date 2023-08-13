using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface IPersonBoardRepository : IRepository<PersonBoard>, IAsyncRepository<PersonBoard>
{
    
}