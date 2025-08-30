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
        var notifications = _repository.GetAllNotifications();

        return Ok(_mapper.Map<IEnumerable<NotificationReadDto>>(notifications));
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
