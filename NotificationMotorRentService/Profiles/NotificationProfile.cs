using AutoMapper;
using NotificationMotorRentService.Dtos;
using NotificationMotorRentService.Models;

namespace NotificationMotorRentService.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationReadDto, Notification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryPersonId, opt => opt.MapFrom(src => src.DeliveryPersonId))
            .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.MotorcycleId))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.Evento, opt => opt.MapFrom(src => src.Evento))
            .ForMember(dest => dest.IsProcessed, opt => opt.MapFrom(src => src.IsProcessed));
    }
}
