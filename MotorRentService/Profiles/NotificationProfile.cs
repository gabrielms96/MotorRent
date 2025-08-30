using AutoMapper;
using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Profiles;

    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationCreateDto, Notification>();
        }
    }

