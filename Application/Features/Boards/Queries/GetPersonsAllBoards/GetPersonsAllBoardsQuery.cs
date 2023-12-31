﻿using Application.Features.Auth.Rules;
using Application.Features.Boards.Dtos;
using Application.Features.Boards.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Boards.Queries.GetPersonsAllBoards;

public class GetPersonsAllBoardsQuery : IRequest<List<GetPersonsBoardDto>>
{
    public int PersonId { get; set; }

    public class GetPersonsAllBoardsQueryHandler : IRequestHandler<GetPersonsAllBoardsQuery, List<GetPersonsBoardDto>>
    {
        private readonly IPersonBoardRepository _personBoardRepository;
        private readonly IMapper _mapper;

        public GetPersonsAllBoardsQueryHandler(IPersonBoardRepository personBoardRepository, IMapper mapper)
        {
            _personBoardRepository = personBoardRepository;
            _mapper = mapper;
        }

        public async Task<List<GetPersonsBoardDto>> Handle(GetPersonsAllBoardsQuery request, CancellationToken cancellationToken)
        {
            List<Board> boards = _personBoardRepository
                .GetListAsync(x => x.PersonId == request.PersonId, include: x => x.Include(x => x.Board), size: 99
                ).Result.Items
                .Select(x => x.Board).ToList();

            List<GetPersonsBoardDto> getPersonsBoardDtos = _mapper.Map<List<GetPersonsBoardDto>>(boards);

            return getPersonsBoardDtos;

        }

    }
}