using Application.Features.Boards.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class JobMappingProfile : Profile
{
    public JobMappingProfile()
    {
        CreateMap<Job, AddedJobDto>();
    }
}