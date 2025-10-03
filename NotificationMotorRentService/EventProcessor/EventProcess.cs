using AutoMapper;
using NotificationMotorRentService.Data;
using NotificationMotorRentService.Dtos;
using NotificationMotorRentService.Models;
using System.Text.Json;

namespace NotificationMotorRentService.EventProcessor
{
    public class EventProcess : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcess(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void Process(string mensagem)
        {
            try
            {
                Console.WriteLine($"Processing message: {mensagem}");
                
                using var scope = _scopeFactory.CreateScope();

                var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var notificationReadDto = JsonSerializer.Deserialize<NotificationReadDto>(mensagem, jsonOptions);
                
                if (notificationReadDto == null)
                {
                    Console.WriteLine("Failed to deserialize notification message");
                    return;
                }

                Console.WriteLine($"Deserialized notification: DeliveryPersonId={notificationReadDto.DeliveryPersonId}, MotorcycleId={notificationReadDto.MotorcycleId}");

                var notification = _mapper.Map<Notification>(notificationReadDto);

                if (notification == null)
                {
                    Console.WriteLine("Failed to map notification");
                    return;
                }

                notification.IsProcessed = true;

                Console.WriteLine($"Storing notification: Message={notification.Message}");
                notificationRepository.StoreNotification(notification);
                Console.WriteLine("Notification processing completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing notification: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
