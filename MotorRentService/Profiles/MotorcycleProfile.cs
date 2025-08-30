using AutoMapper;
using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Profiles;

public class MotorcycleProfile : Profile
{
    public MotorcycleProfile()
    {
        CreateMap<Motorcycle, MotorcycleDto>();
        CreateMap<MotorcycleCreateDto, Motorcycle>();
    }
}
