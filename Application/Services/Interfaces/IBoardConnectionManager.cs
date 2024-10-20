using Domain.Configurations;
using Domain.Entities.Concrete;

namespace Application.Services.Interfaces;

public interface IBoardConnectionManager
{
    void AddConnection(int boardId, IBoardConnection connection);
    void RemoveConnection(int boardId, string connectionId);
    Task SendUpdateToBoard(int boardId, Board updatedBoard);
    public List<IBoardConnection> GetConnections(int boardId);

}