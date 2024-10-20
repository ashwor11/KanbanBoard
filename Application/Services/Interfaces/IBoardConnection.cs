namespace Application.Services.Interfaces;

public interface IBoardConnection
{
    string ConnectionId { get; }
    int BoardId { get; }

    Task SendMessage(string message);
}