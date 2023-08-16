using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.ChangeJobFeedbackCommand;

public class ChangeJobFeedbackCommand : IRequest
{
    public ChangeJobFeedbackDto ChangeJobFeedbackDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeJobFeedbackCommandHandler : IRequestHandler<ChangeJobFeedbackCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeJobFeedbackCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeJobFeedbackCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.ChangeJobFeedbackDto.BoardId);
            _boardBusinessRules.DoesBoardCardJobAndTheJobFeedbackExist(board, request.ChangeJobFeedbackDto.CardId,
                request.ChangeJobFeedbackDto.JobId, request.ChangeJobFeedbackDto.JobFeedbackId);

            JobFeedback jobFeedback = board.Cards.First(x => x.Id == request.ChangeJobFeedbackDto.CardId).Jobs
                .First(x => x.Id == request.ChangeJobFeedbackDto.JobId).Feedbacks
                .First(x => x.Id == request.ChangeJobFeedbackDto.JobFeedbackId);
            jobFeedback.Content = request.ChangeJobFeedbackDto.Content;

            await _boardRepository.UpdateAsync(board);
        }
    }
}