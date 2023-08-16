using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.DeleteJobCommand;

public class DeleteJobCommand : IRequest, IValidationRequest
{
    public DeleteJobDto DeleteJobDto { get; set; }
    public int PersonId { get; set; }

    public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public DeleteJobCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.DeleteJobDto.BoardId);
            _boardBusinessRules.DoesBoardCardAndTheJobExist(board,request.DeleteJobDto.CardId, request.DeleteJobDto.JobId);
            Card card = board.Cards.First(x => x.Id == request.DeleteJobDto.CardId);
            Job jobToDelete = card.Jobs.First(x => x.Id == request.DeleteJobDto.JobId);

            card.Jobs.Remove(jobToDelete);

            await _boardRepository.UpdateAsync(board);

        }
    }
}