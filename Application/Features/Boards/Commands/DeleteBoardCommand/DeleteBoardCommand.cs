using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommand : IRequest<DeletedBoardDto>
{
    public int BoardId { get; set; }
    public int PersonId { get; set; }
    public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, DeletedBoardDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardService _boardService;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public DeleteBoardCommandHandler(IBoardRepository boardRepository, IBoardService boardService, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardService = boardService;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<DeletedBoardDto> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetAsync(x => x.Id == request.BoardId)!;
            _boardBusinessRules.DoesBoardExist(board);

            Board deletedBoard = await _boardRepository.DeleteAsync(board);

            DeletedBoardDto deletedBoardDto = _mapper.Map<DeletedBoardDto>(deletedBoard);

            return deletedBoardDto; 


        }
    }
}