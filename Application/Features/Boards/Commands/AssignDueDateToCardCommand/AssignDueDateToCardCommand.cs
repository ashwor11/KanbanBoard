using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AssignDueDateToCardCommand;

public class AssignDueDateToCardCommand : IRequest, IValidationRequest
{
    public AssignDueDateToCardDto AssignDueDateToCardDto { get; set; }
    public int PersonId { get; set; }

    public class AssignDueDateToCardCommandHandler : IRequestHandler<AssignDueDateToCardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public AssignDueDateToCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(AssignDueDateToCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(request.AssignDueDateToCardDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board, request.AssignDueDateToCardDto.CardId);
            Card card = board.Cards.First(x => x.Id == request.AssignDueDateToCardDto.CardId);
            card.AssignDueDate(request.AssignDueDateToCardDto.DueDate);

            await _boardRepository.UpdateAsync(board);
        }
    }
}