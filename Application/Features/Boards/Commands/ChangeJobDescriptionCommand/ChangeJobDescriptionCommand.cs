using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.ChangeJobDescriptionCommand;

public class ChangeJobDescriptionCommand : IRequest
{
    public ChangeJobDescriptionDto ChangeJobDescriptionDto { get; set; }
    public int PersonId { get; set; }

    public class ChangeJobDescriptionCommandHandler : IRequestHandler<ChangeJobDescriptionCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public ChangeJobDescriptionCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task Handle(ChangeJobDescriptionCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.ChangeJobDescriptionDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);


            Job job = board.Cards.SelectMany(x => x.Jobs)
                .FirstOrDefault(x => x.Id == request.ChangeJobDescriptionDto.JobId);
            _boardBusinessRules.IsNull(job);

            job.JobDescription = request.ChangeJobDescriptionDto.JobDescription;

            await _boardRepository.UpdateAsync(board);

            return;
        }
    }
}