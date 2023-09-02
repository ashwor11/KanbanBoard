using Application.Features.Boards.Dtos;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Validation;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardCommand : IRequest<CreatedBoardDto>, IValidationRequest 
{
    public BoardToCreateDto BoardToCreateDto { get; set; }
    public int PersonId { get; set; }

    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, CreatedBoardDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IPersonService _personService;

        public CreateBoardCommandHandler(IBoardRepository boardRepository, IPersonService personService)
        {
            _boardRepository = boardRepository;
            _personService = personService;
        }

        public async Task<CreatedBoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            Board board = new()
            {
                CreatorUserId = request.PersonId, Name = request.BoardToCreateDto.Name,
                Description = request.BoardToCreateDto.Description
            };

            Board createdBoard = await _boardRepository.CreateAsync(board);

            await _personService.AddBoardToPerson(request.PersonId, createdBoard.Id);

            return new CreatedBoardDto()
                { Id = createdBoard.Id, Name = createdBoard.Name, Description = createdBoard.Description };
        }
    }
}