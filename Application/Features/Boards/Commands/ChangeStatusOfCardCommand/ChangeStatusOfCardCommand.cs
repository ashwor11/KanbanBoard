using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Validation;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities.Concrete;
using Domain.Entities.Enums;
using MediatR;

namespace Application.Features.Boards.Commands.ChangeStatusOfCardCommand;

public class ChangeStatusOfCardCommand : IRequest, IValidationRequest
{
    public ChangeCardStatusDto ChangeCardStatusDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeStatusOfCardCommandHandler : IRequestHandler<ChangeStatusOfCardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeStatusOfCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeStatusOfCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(request.ChangeCardStatusDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.ChangeCardStatusDto.CardId);
            ChangeStatus(board.Cards.FirstOrDefault(x=>x.Id == request.ChangeCardStatusDto.CardId),request.ChangeCardStatusDto.Status);
            await _boardRepository.UpdateAsync(board);

        }

        private void ChangeStatus(Card card, string status)
        {
            switch (status)
            {
                case "ToDo":
                    
                    break;
                case "InProgress":
                    card.Status = CardStatus.InProgress;
                    break;
                case "Review":
                    card.Status = CardStatus.Review;
                    break;
                case "Backlog":
                    card.Status = CardStatus.Backlog;
                    break;
                case "Done":
                    card.Status = CardStatus.Done;
                    break;
                default:
                    throw new BusinessException("It is not a valid statement");

            }
        }
    }
}