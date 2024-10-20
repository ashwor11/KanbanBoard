using Core.Persistence.Repositories;
using Domain.Entities.Concrete;

namespace Application.Repositories;

public interface IBoardRepository : IRepository<Board>, IAsyncRepository<Board>
{
    public Task<Board> GetWholeBoardAsync(int? boardId);
    public Task<Board> GetBoardWithCardsAsync(int? boardId);
    public Task<Board> GetWholeBoardWithPersons(int boardId);

}