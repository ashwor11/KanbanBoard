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
            _boardBusinessRules.DoesBoardExist(board);

            Card card = board.Cards
                .Where(x => x.Feedbacks.Any(x => x.Id == request.DeleteCardFeedbackDto.CardFeedbackId))
                .FirstOrDefault();
            _boardBusinessRules.IsNull(card);
            CardFeedback cardFeedback = card.Feedbacks.FirstOrDefault(x => x.Id == request.DeleteCardFeedbackDto.CardFeedbackId);

            card.Feedbacks.Remove(cardFeedback);

            await _boardRepository.UpdateAsync(board);
        }
    }
}