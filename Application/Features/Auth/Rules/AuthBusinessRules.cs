using Application.Repositories;
using Application.Services.Abstract;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities.Concrete;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules 
{
    private readonly IPersonService _personService;

    public AuthBusinessRules(IPersonService personService)
    {
        _personService = personService;
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


}