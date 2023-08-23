using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.MarkJobAsDone;

public class MarkJobAsDoneCommand : IRequest
{
    public MarkJobDto MarkJobDto { get; set; }
    public int PersonId { get; set; }

    public class MarkJobAsDoneCommandHandler : IRequestHandler<MarkJobAsDoneCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public MarkJobAsDoneCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(MarkJobAsDoneCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.MarkJobDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            Job job = board.Cards.SelectMany(x => x.Jobs).FirstOrDefault(x => x.Id == request.MarkJobDto.JobId);
            _boardBusinessRules.IsNull(job);

            job.IsDone = true;

            await _boardRepository.UpdateAsync(board);
        }
    }
}