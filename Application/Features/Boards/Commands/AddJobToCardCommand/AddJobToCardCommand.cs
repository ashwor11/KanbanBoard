using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AddJobToCardCommand;

public class AddJobToCardCommand : IRequest<AddedJobDto>
{
    public AddJobToCardDto AddJobToCardDto { get; set; } 
    public int PersonId { get; set; }

    public class AddJobToCardCommandHandler : IRequestHandler<AddJobToCardCommand,AddedJobDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public AddJobToCardCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<AddedJobDto> Handle(AddJobToCardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.AddJobToCardDto.BoardId);
            _boardBusinessRules.DoesBoardAndTheCardExist(board,request.AddJobToCardDto.CardId);
            Card card = board.Cards.FirstOrDefault(x => x.Id == request.AddJobToCardDto.CardId);
            
            card.AddJob();
            

            await _boardRepository.UpdateAsync(board);
            Job addedJob = card.Jobs[^1];

            AddedJobDto addedJobDto = _mapper.Map<AddedJobDto>(addedJob);

            return addedJobDto;

        }
    }
}