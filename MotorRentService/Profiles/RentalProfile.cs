using AutoMapper;
using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<Rental, RentalDto>();
            CreateMap<RentalCreateDto, Rental>();
        }
    }
}
