using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BoardRepository : EfRepositoryBase<KanbanDbContext, Board>, IBoardRepository
{
    public BoardRepository(KanbanDbContext context) : base(context)
    {
    }

    public async Task<Board> GetWholeBoardAsync(int boardId)
    {
        Board board = await GetAsync(x => x.Id == boardId,
            include: x =>
                x.Include(x => x.Cards).ThenInclude(x => x.Feedbacks).Include(x => x.Cards).ThenInclude(x => x.Jobs).ThenInclude(x=>x.Feedbacks));
        return board;
    }

    public async Task<Board> GetBoardWithCardsAsync(int boardId)
    {
        Board board = await GetAsync(x => x.Id == boardId,
            include: x =>
                x.Include(x => x.Cards));
        return board;
    }

    public async Task<Board> GetWholeBoardWithPersons(int boardId)
    {
        Board board = await GetAsync(x => x.Id == boardId,
            include: x =>
                x.Include(x => x.Cards).ThenInclude(x => x.Feedbacks).
                    Include(x => x.Cards).ThenInclude(x => x.Jobs).ThenInclude(x=>x.Feedbacks).
                    Include(x=>x.PersonBoards).ThenInclude(x=>x.Person));
        return board;
    }
}