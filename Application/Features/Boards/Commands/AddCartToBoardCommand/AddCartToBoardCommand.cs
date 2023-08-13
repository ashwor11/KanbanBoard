using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AddCartToBoardCommand;

public class AddCartToBoardCommand : IRequest<AddedCardDto>
{
    public CardToAddDto CardToAddDto { get; set; }
    public int PersonId { get; set; }

    public class AddCartToBoardCommandHandler : IRequestHandler<AddCartToBoardCommand,AddedCardDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public AddCartToBoardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<AddedCardDto> Handle(AddCartToBoardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.CardToAddDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            board.Cards!.Add(new Card());

            board = await _boardRepository.UpdateAsync(board);
            Card addedCard = board.Cards![^1];

            AddedCardDto addedCardDto = _mapper.Map<AddedCardDto>(addedCard);
            return addedCardDto;

        }
    }
}