using Application.Features.Boards.Dtos;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities.Concrete;
using Persistence.Repositories;

namespace WebAPI.Middlewares;

public class BoardUpdaterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEventPublisher _eventPublisher;
    public BoardUpdaterMiddleware(RequestDelegate next, IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
        _next = next;
        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        var routeData = context.GetRouteData();
        if(routeData.Values.ContainsKey("boardId"))
        {
            
            using (var scope = context.RequestServices.CreateScope())
            {
                int? boardId = GetBoardId(context);
                IBoardRepository boardRepository= scope.ServiceProvider.GetRequiredService<IBoardRepository>();
                Board board = await boardRepository.GetWholeBoardAsync(boardId);
                await _eventPublisher.PublishBoardUpdate(board);
            }
            
        }
    }

    private int? GetBoardId(HttpContext context)
    {
        var path = context.Request.Path;
        if (!path.HasValue) return null;

        var segments = path.Value.Split('/');

        //api/boards/{boardId}/cards

        if (segments.Length < 3) return null;
        if (!int.TryParse(segments[3], out var boardId)) return null;
        return boardId;
    }
}