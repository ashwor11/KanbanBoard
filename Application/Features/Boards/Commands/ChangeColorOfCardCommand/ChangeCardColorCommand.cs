using System.Security.AccessControl;
using System.Security.Principal;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;
using Org.BouncyCastle.Security;

namespace Application.Features.Boards.Commands.ChangeColorOfCardCommand;

public class ChangeCardColorCommand : IRequest
{
    public ChangeCardColorDto ChangeCardColorDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeCardColorCommandHandler : IRequestHandler<ChangeCardColorCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeCardColorCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeCardColorCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.ChangeCardColorDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            Card card = board.Cards.FirstOrDefault(x=>x.Id == request.ChangeCardColorDto.CardId);
            _boardBusinessRules.IsNull(card);

            card.ChangeColor(request.ChangeCardColorDto.ColorInHex);

            await _boardRepository.UpdateAsync(board);
        }
    }
}