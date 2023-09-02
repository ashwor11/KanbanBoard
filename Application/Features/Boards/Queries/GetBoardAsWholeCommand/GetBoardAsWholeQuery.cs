using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using Domain.Entities.Enums;
using MediatR;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeQuery : IRequest<GetBoardByIdDto>, IValidationRequest
{
    public int BoardId { get; set; }
    public int PersonId { get; set; }

    public class GetBoardAsWholeCommandHandler : IRequestHandler<GetBoardAsWholeQuery, GetBoardByIdDto>
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

        public async Task<GetBoardByIdDto> Handle(GetBoardAsWholeQuery request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardWithPersons(request.BoardId);
            _boardBusinessRules.DoesBoardExist(board);

            GetBoardByIdDto getBoardByIdDto = _mapper.Map<GetBoardByIdDto>(board);

            return getBoardByIdDto; 
        }
    }
}