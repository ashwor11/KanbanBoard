using Application.Features.Boards.Commands.MarkJobAsDone;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.MarkJobAsUnDoneCommand;

public class MarkJobAsUnDoneCommand : IRequest
{
    public MarkJobDto MarkJobDto { get; set; }
    public int PersonId { get; set; }

    public class MarkJobAsUnDoneCommandHandler : IRequestHandler<MarkJobAsUnDoneCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public MarkJobAsUnDoneCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(MarkJobAsUnDoneCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.MarkJobDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            Job job = board.Cards.SelectMany(x => x.Jobs).FirstOrDefault(x => x.Id == request.MarkJobDto.JobId);
            _boardBusinessRules.IsNull(job);

            job.IsDone = false;

            await _boardRepository.UpdateAsync(board);
        }
    }
}
