using AutoMapper;
using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Profiles;

public class RentalPlanProfile : Profile
{
    public RentalPlanProfile()
    {
        CreateMap<RentalPlan, RentalPlanDto>();
        CreateMap<RentalPlanCreateDto, RentalPlan>();
    }
}

