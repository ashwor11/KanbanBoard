using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Application.Pipelines.Validation;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities.Concrete;
using MediatR;

namespace Application.Features.Auth.Commands.LoginCommand;

public class LoginCommand : IRequest<LoggedInPersonDto>
{
    public PersonToLoginDto PersonToLoginDto { get; set; }  

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInPersonDto>, IValidationRequest
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly ITokenHelper _tokenHelper;

        public LoginCommandHandler(IPersonRepository personRepository, IPersonService personService, AuthBusinessRules authBusinessRules, ITokenHelper tokenHelper)
        {
            _personRepository = personRepository;
            _personService = personService;
            _authBusinessRules = authBusinessRules;
            _tokenHelper = tokenHelper;
        }

        public async Task<LoggedInPersonDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.IsEmailAlreadyRegistered(request.PersonToLoginDto.Email);

            Person? person = await _personRepository.GetAsync(x => x.Email == request.PersonToLoginDto.Email)!;

            HashingHelper.VerifyPasswordHash(request.PersonToLoginDto.Password, person.PasswordHash,
                person.PasswordSalt);

            _personService.DoesGivenPasswordMatchesWithPersonsPassword(request.PersonToLoginDto.Password, person);

            IList<OperationClaim> operationClaims = await _personService.GetOperationClaimsForPerson(person);

            

            AccessToken accessToken = _tokenHelper.CreateToken(person, operationClaims);
            LoggedInPersonDto loggedInPersonDto = new()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                AccessToken = accessToken
            };

            return loggedInPersonDto;
        }
    }
}