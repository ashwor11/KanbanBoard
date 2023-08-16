using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.DeleteJobFeedbackCommand;

public class DeleteJobFeedbackCommand : IRequest, IValidationRequest
{
    public DeleteJobFeedbackDto DeleteJobFeedbackDto { get; set; }
    public int PersonId { get; set; }

    public class DeleteJobFeedbackCommandHandler : IRequestHandler<DeleteJobFeedbackCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public DeleteJobFeedbackCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(DeleteJobFeedbackCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.DeleteJobFeedbackDto.BoardId);
            _boardBusinessRules.DoesBoardCardJobAndTheJobFeedbackExist(board, request.DeleteJobFeedbackDto.CardId, request.DeleteJobFeedbackDto.JobId, request.DeleteJobFeedbackDto.JobFeedbackId);
            Card card = board.Cards.First(x => x.Id == request.DeleteJobFeedbackDto.CardId);
            Job job = card.Jobs.First(x => x.Id == request.DeleteJobFeedbackDto.JobId);
            JobFeedback jobFeedbackToDelete = job.Feedbacks.First(x => x.Id == request.DeleteJobFeedbackDto.JobFeedbackId);

            job.Feedbacks.Remove(jobFeedbackToDelete);
        }
    }
}