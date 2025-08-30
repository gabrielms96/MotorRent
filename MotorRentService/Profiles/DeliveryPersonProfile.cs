using AutoMapper;
using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Profiles;

public class DeliveryPersonProfile : Profile
{

    public DeliveryPersonProfile()
    {
        CreateMap<DeliveryPerson, DeliveryPersonDto>();
        CreateMap<DeliveryPersonCreateDto, DeliveryPerson>();
    }

}

