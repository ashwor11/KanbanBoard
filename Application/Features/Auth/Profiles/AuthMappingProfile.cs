using Application.Features.Auth.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Auth.Profiles;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<PersonToRegisterDto, Person>();
    }
}