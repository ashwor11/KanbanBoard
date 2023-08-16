using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeCommand : IRequest<GetWholeBoardDto>, IValidationRequest
{
    public int BoardId { get; set; }
    public int PersonId { get; set; }

    public class GetBoardAsWholeCommandHandler : IRequestHandler<GetBoardAsWholeCommand,GetWholeBoardDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public GetBoardAsWholeCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<GetWholeBoardDto> Handle(GetBoardAsWholeCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardWithPersons(request.BoardId);
            _boardBusinessRules.DoesBoardExist(board);

            GetWholeBoardDto toReturnBoardDto = _mapper.Map<GetWholeBoardDto>(board);

            return toReturnBoardDto;
        }
    }
}