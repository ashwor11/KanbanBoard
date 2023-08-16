using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;

public class RemoveAssignedDueDateFromCardCommand : IRequest , IValidationRequest
{
    public RemoveAssignedDueDateFromCardDto RemoveAssignedDueDateFromCardDto { get; set; }
    public int PersonId { get; set; }

    public class RemoveAssignedDueDateFromCardCommandHandler : IRequestHandler<RemoveAssignedDueDateFromCardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public RemoveAssignedDueDateFromCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(RemoveAssignedDueDateFromCardCommand request, CancellationToken cancellationToken)
        {
            Board board =
                await _boardRepository.GetBoardWithCardsAsync(request.RemoveAssignedDueDateFromCardDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.RemoveAssignedDueDateFromCardDto.CardId);
            Card card = board.Cards.First(x => x.Id == request.RemoveAssignedDueDateFromCardDto.CardId);
            card.RemoveDueDate();

            await _boardRepository.UpdateAsync(board);
        }
    }
}