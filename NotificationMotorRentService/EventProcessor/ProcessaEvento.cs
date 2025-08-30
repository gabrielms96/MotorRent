using AutoMapper;
using NotificationMotorRentService.Data;
using NotificationMotorRentService.Dtos;
using NotificationMotorRentService.Models;
using System.Text.Json;

namespace NotificationMotorRentService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateScope();

            var notificationRepository = scope.ServiceProvider.GetRequiredService<NotificationRepository>();

            var notificationReadDto = JsonSerializer.Deserialize<NotificationReadDto>(mensagem);

            var notification = _mapper.Map<Notification>(notificationReadDto);

            notification.IsProcessed = true;

            notificationRepository.StoreNotification(notification);
        }
    }
}
