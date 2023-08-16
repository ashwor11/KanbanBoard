using Application.Repositories;
using Application.Services.Abstract;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities.Concrete;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules 
{
    private readonly IPersonService _personService;
    private readonly IPersonRepository _personRepository;

    public AuthBusinessRules(IPersonService personService, IPersonRepository personRepository)
    {
        _personService = personService;
        _personRepository = personRepository;
    }

    public async Task IsEmailAlreadyRegistered(string email)
    {
        Person? person = await  _personService.GetPersonWithEmail(email)!;
        if (person == null) throw new BusinessException("There is no such a email that registered.");
    }

    public async Task IsEmailFreeToTaken(string email)
    {
        Person? person = await _personService.GetPersonWithEmail(email)!;
        if (person != null) throw new BusinessException("There is already a user with this email.");
    }


    public async Task DoesPersonExist(int personId)
    {
        Person? person = await _personRepository.GetAsync(x => x.Id == personId);
        if (person == null) throw new BusinessException("The specified person does not exist.");
    }
}