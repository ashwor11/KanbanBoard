using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.ChangeCardNameCommand;

public class ChangeCardNameCommand : IRequest, IValidationRequest
{
    public ChangeCardNameDto ChangeCardNameDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeCardNameCommandHandler : IRequestHandler<ChangeCardNameCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeCardNameCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeCardNameCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(request.ChangeCardNameDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board, request.ChangeCardNameDto.CardId);
            board.Cards.First(x=>x.Id == request.ChangeCardNameDto.CardId).ChangeCardName(request.ChangeCardNameDto.Name);
            await _boardRepository.UpdateAsync(board);
        }
    }
}