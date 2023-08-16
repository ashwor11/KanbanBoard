

using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;

public class RemoveAssignedPersonFromCardCommand : IRequest, IValidationRequest
{
    public RemoveAssignedPersonFromCardDto RemoveAssignedPersonFromCardDto { get; set; }
    public int PersonId { get; set; }
    public class RemoveAssignedPersonFromCardCommandHandler : IRequestHandler<RemoveAssignedPersonFromCardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public RemoveAssignedPersonFromCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(RemoveAssignedPersonFromCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(request.RemoveAssignedPersonFromCardDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.RemoveAssignedPersonFromCardDto.CardId);
            Card card = board.Cards.First(x => x.Id == request.RemoveAssignedPersonFromCardDto.CardId);
            card.RemoveAssignedPerson();
            await _boardRepository.UpdateAsync(board);
        }
    }
}

