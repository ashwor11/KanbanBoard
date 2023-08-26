﻿using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Validation;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Auth.Commands.RegisterCommand;

public class RegisterPersonCommand : IRequest<RegisteredPersonDto>, IValidationRequest
{
    public PersonToRegisterDto PersonToRegisterDto { get; set; }

    public class RegisterPersonCommandHandler : IRequestHandler<RegisterPersonCommand, RegisteredPersonDto>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly AuthBusinessRules _authBusinessRules;

        public RegisterPersonCommandHandler(IPersonRepository personRepository, ITokenHelper tokenHelper, IMapper mapper, AuthBusinessRules authBusinessRules)
        {
            _personRepository = personRepository;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RegisteredPersonDto> Handle(RegisterPersonCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.IsEmailFreeToTaken(request.PersonToRegisterDto.Email);

            Person person = _mapper.Map<Person>(request.PersonToRegisterDto);

            byte[] passwordSalt, passwordHash;

            HashingHelper.CreatePasswordHash(request.PersonToRegisterDto.Password,out passwordHash,out passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            await _personRepository.CreateAsync(person);

            AccessToken accessToken = _tokenHelper.CreateToken(person, new List<OperationClaim>());

            RegisteredPersonDto registeredPersonDto = new()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                AccessToken = accessToken
            };

            return registeredPersonDto;

        }
    }
}