#region Mainetence
/*
Comment: Added try catch and fixed saving notification.
Updated: 10/03/2025 13:15
Author:  Gabriel MS
*/
#endregion
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationMotorRentService.Data;
using NotificationMotorRentService.Dtos;
using System.Collections.Generic;

namespace NotificationMotorRentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _repository;
    private readonly IMapper _mapper;

    public NotificationController(INotificationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("GetAllNotifications")]
    public ActionResult<IEnumerable<NotificationReadDto>> GetAllNotifications()
    {
        try
        {
            var notifications = _repository.GetAllNotifications();
            var notificationsList = notifications.ToList();
            
            Console.WriteLine($"Retrieved {notificationsList.Count} notifications from database");
            
            var result = _mapper.Map<IEnumerable<NotificationReadDto>>(notificationsList);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving notifications: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("GetNotificationDeliveryPersonId")]
    public ActionResult<IEnumerable<NotificationReadDto>> GetNotificationDeliveryPersonId(string deliveryPersonId)
    {
        _repository.GetNotificationDeliveryPersonId(deliveryPersonId);
        return Ok();
    }

    [HttpPost("GetNotificationDeliveryMotorcycleId")]
    public ActionResult<IEnumerable<NotificationReadDto>> GetNotificationDeliveryMotorcycleId(string motorcycleId)
    {
        _repository.GetNotificationDeliveryMotorcycleId(motorcycleId);
        return Ok();
    }
}
