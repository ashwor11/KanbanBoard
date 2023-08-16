using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.DeleteCardCommand;

public class DeleteCardCommand : IRequest, IValidationRequest
{
    public DeleteCardDto DeleteCardDto { get; set; }
    public int PersonId { get; set; }

    public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public DeleteCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.DeleteCardDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.DeleteCardDto.CardId);

            Card card = board.Cards.First(x => x.Id == request.DeleteCardDto.CardId);
            board.Cards.Remove(card);

            await _boardRepository.UpdateAsync(board);
        }
    }
}