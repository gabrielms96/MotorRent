#region Mainetence
/*
Comment: Created.
Created: 08/31/2024 21:10
Author:  Gabriel MS
*/
#endregion
using MotorRentService.RabbitMqClient;
using System.Text.Json;

namespace MotorRentService.Services;

public class EventPublisher : IEventPublisher
{
    private readonly IRabbitMqClient _rabbitMqClient;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IRabbitMqClient rabbitMqClient, ILogger<EventPublisher> logger)
    {
        _rabbitMqClient = rabbitMqClient;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            _logger.LogInformation("Publishing event of type {EventType}", typeof(T).Name);
            
            // For now, we'll serialize to JSON and log - this can be extended to use RabbitMQ
            var eventJson = JsonSerializer.Serialize(@event);
            _logger.LogInformation("Event published: {Event}", eventJson);
            
            // Note: The RabbitMqClient currently only handles NotificationDto
            // This is a simple implementation that logs the event
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event of type {EventType}", typeof(T).Name);
            throw;
        }
    }
}
