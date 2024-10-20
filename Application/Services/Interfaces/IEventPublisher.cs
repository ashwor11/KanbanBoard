using Domain.Entities.Concrete;

namespace Application.Services.Interfaces;

public interface IEventPublisher
{
    Task PublishBoardUpdate(Board updatedBoard);

}