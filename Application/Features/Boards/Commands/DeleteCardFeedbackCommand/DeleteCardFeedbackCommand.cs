using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.DeleteCardFeedbackCommand;

public class DeleteCardFeedbackCommand : IRequest, IValidationRequest
{
    public DeleteCardFeedbackDto DeleteCardFeedbackDto { get; set; }
    public int PersonId { get; set; }

    public class DeleteCardFeedbackCommandHandler : IRequestHandler<DeleteCardFeedbackCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public DeleteCardFeedbackCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(DeleteCardFeedbackCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.DeleteCardFeedbackDto.BoardId);
            _boardBusinessRules.DoesBoardCardAndCardFeedbackExist(board, request.DeleteCardFeedbackDto.CardId, request.DeleteCardFeedbackDto.CardFeedbackId);

            Card card = board.Cards.First(x => x.Id == request.DeleteCardFeedbackDto.CardId);
            CardFeedback cardFeedback = card.Feedbacks.First(x=>x.Id == request.DeleteCardFeedbackDto.CardFeedbackId);
            card.Feedbacks.Remove(cardFeedback);

            await _boardRepository.UpdateAsync(board);
        }
    }
}