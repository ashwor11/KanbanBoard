using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.LeaveBoardCommand;

public class LeaveBoardCommand : IRequest 
{
    public LeaveBoardDto LeaveBoardDto { get; set; }
    public int PersonId { get; set; }

    public class LeaveBoardCommandHandler : IRequestHandler<LeaveBoardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public LeaveBoardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(LeaveBoardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardWithPersons(request.LeaveBoardDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            _boardBusinessRules.IsPersonAMemberOfBoard(board, request.PersonId);

            board.RemovePersonFromBoard(request.PersonId);

            await _boardRepository.UpdateAsync(board);

        }
    }
}