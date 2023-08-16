using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.ChangeCardFeedBackCommand;

public class ChangeCardFeedbackCommand : IRequest
{
    public ChangeCardFeedbackDto ChangeCardFeedbackDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeCardFeedbackCommandHandler : IRequestHandler<ChangeCardFeedbackCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeCardFeedbackCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeCardFeedbackCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.ChangeCardFeedbackDto.BoardId);
            _boardBusinessRules.DoesBoardCardAndCardFeedbackExist(board, request.ChangeCardFeedbackDto.CardId, request.ChangeCardFeedbackDto.CardFeedbackId);

            CardFeedback cardFeedback = board.Cards.First(x => x.Id == request.ChangeCardFeedbackDto.CardId).Feedbacks
                .First(x => x.Id == request.ChangeCardFeedbackDto.CardFeedbackId);

            cardFeedback.Content = request.ChangeCardFeedbackDto.Content;

            await _boardRepository.UpdateAsync(board);

        }
    }
}