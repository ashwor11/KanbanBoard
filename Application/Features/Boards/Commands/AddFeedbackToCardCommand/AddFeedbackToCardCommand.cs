using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AddFeedbackToCardCommand;

public class AddFeedbackToCardCommand: IRequest<AddedCardFeedbackDto>
{
    public CardFeedbackToAddDto CardFeedbackToAddDto { get; set; }
    public int PersonId { get; set; }


    public class AddFeedbackToCardCommandHandler : IRequestHandler<AddFeedbackToCardCommand, AddedCardFeedbackDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public AddFeedbackToCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<AddedCardFeedbackDto> Handle(AddFeedbackToCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.CardFeedbackToAddDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board, request.CardFeedbackToAddDto.CardId);

            Card card = board.Cards.First(x => x.Id == request.CardFeedbackToAddDto.CardId);
            card.AddFeedback(request.PersonId,request.CardFeedbackToAddDto.Content);

            await _boardRepository.UpdateAsync(board);

            CardFeedback addedCardFeedback = card.Feedbacks[^1];

            AddedCardFeedbackDto addedCardFeedbackDto= _mapper.Map<AddedCardFeedbackDto>(addedCardFeedback);
            return addedCardFeedbackDto;
        }
    }
}