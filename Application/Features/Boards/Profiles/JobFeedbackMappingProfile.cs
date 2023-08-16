using Application.Features.Boards.Dtos;
using AutoMapper;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Profiles;

public class JobFeedbackMappingProfile : Profile
{
    public JobFeedbackMappingProfile()
    {
        CreateMap<JobFeedback, AddedJobFeedbackDto>();
    }
}