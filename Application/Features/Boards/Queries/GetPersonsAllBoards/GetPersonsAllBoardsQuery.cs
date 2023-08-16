using Application.Features.Auth.Rules;
using Application.Features.Boards.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Boards.Queries.GetPersonsAllBoards;

public class GetPersonsAllBoardsQuery : IRequest<GetPersonsAllBoardsModel>
{
    public int PersonId { get; set; }

    public class GetPersonsAllBoardsQueryHandler : IRequestHandler<GetPersonsAllBoardsQuery, GetPersonsAllBoardsModel>
    {
        private readonly IPersonBoardRepository _personBoardRepository;
        private readonly IMapper _mapper;

        public GetPersonsAllBoardsQueryHandler(IPersonBoardRepository personBoardRepository, IMapper mapper)
        {
            _personBoardRepository = personBoardRepository;
            _mapper = mapper;
        }

        public async Task<GetPersonsAllBoardsModel> Handle(GetPersonsAllBoardsQuery request, CancellationToken cancellationToken)
        {
            List<Board> boards = _personBoardRepository
                .GetListAsync(x => x.PersonId == request.PersonId, include: x => x.Include(x => x.Board)).Result.Items
                .Select(x => x.Board).ToList();

            GetPersonsAllBoardsModel getPersonsAllBoardsModel = _mapper.Map<GetPersonsAllBoardsModel>(boards);

            return getPersonsAllBoardsModel;

        }

    }
}