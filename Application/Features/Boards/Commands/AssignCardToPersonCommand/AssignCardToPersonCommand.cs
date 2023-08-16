using Application.Features.Auth.Rules;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AssignCardToPersonCommand;

public class AssignCardToPersonCommand : IRequest , IValidationRequest
{
    public AssignCardToPersonDto AssignCardToPersonDto { get; set; }
    public int PersonId { get; set; }

    public class AssignCardToPersonCommandHandler : IRequestHandler<AssignCardToPersonCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly AuthBusinessRules _authBusinessRules;

        public AssignCardToPersonCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, AuthBusinessRules authBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _authBusinessRules = authBusinessRules;
        }

        public async Task Handle(AssignCardToPersonCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(request.AssignCardToPersonDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.AssignCardToPersonDto.CardId);
            await _authBusinessRules.DoesPersonExist(request.AssignCardToPersonDto.PersonId);
            Card card = board.Cards.First(x => x.Id == request.AssignCardToPersonDto.CardId);
            card.AssignCardToPerson(request.PersonId);

            await _boardRepository.UpdateAsync(board);
        }
    }
}