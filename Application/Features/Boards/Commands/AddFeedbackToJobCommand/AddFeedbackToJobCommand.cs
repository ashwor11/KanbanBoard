using System.Security.Cryptography.X509Certificates;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Boards.Commands.AddFeedbackToJobCommand;

public class AddFeedbackToJobCommand : IRequest<AddedJobFeedbackDto>
{
    public JobFeedbackToAddDto JobFeedbackToAddDto { get; set; }
    public int PersonId { get; set; }

    public class AddFeedbackToJobCommandHandler : IRequestHandler<AddFeedbackToJobCommand,AddedJobFeedbackDto>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;
        private readonly IMapper _mapper;

        public AddFeedbackToJobCommandHandler(IBoardRepository boardRepository, BoardBusinessRules boardBusinessRules, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
            _mapper = mapper;
        }

        public async Task<AddedJobFeedbackDto> Handle(AddFeedbackToJobCommand request, CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetWholeBoardAsync(request.JobFeedbackToAddDto.BoardId);
            _boardBusinessRules.DoesBoardExist(board);
            Job job = board.Cards.SelectMany(x => x.Jobs)
                .FirstOrDefault(x => x.Id == request.JobFeedbackToAddDto.JobId);
            _boardBusinessRules.IsNull(job);

            job.AddFeedback(request.PersonId,request.JobFeedbackToAddDto.Content);

            await _boardRepository.UpdateAsync(board);

            JobFeedback addedJobFeedback = job.Feedbacks[^1];

            AddedJobFeedbackDto addedJobFeedbackDto = _mapper.Map<AddedJobFeedbackDto>(addedJobFeedback);
            return addedJobFeedbackDto;
        }
    }
}