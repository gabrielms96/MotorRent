using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.WebEncoders.Testing;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using MotorRentService.Motorcycles.Commands.CreateMotorcycle;
using MotorRentService.RabbitMqClient;

namespace MotorRentService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleRepository _repository;
    private readonly IMapper _mapper;
    private IRabbitMqClient _rabbitMqClient;
    private readonly IMediator _mediator;
    private readonly ILogger<MotorcycleController> _logger;

    public MotorcycleController(
        IMotorcycleRepository repository,
        IMapper mapper,
        IRabbitMqClient rabbitMqClient,
        IMediator mediator,
        ILogger<MotorcycleController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _rabbitMqClient = rabbitMqClient;
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet("GetAllMotorcycles")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<MotorcycleDto>> GetAllMotorcycles()
    {
        _logger.LogInformation("START - Getting all motorcycle with ID");

        var motorcycle = _repository.GetAllAsync(); ;
        if (motorcycle != null)
        {
            _logger.LogInformation("END - Getting all motorcycle: {count}", motorcycle.Result.Count());
            return Ok(_mapper.Map<IEnumerable<MotorcycleDto>>(motorcycle));
        }
        _logger.LogInformation("END - Getting all motorcycle: NO RECORDS");
        return NotFound();
    }

    /// <summary>
    /// Gets a motorcycle by ID
    /// </summary>
    /// <param name="id">Motorcycle ID</param>
    /// <returns>Motorcycle details</returns>
    [HttpGet("{id}", Name = "GetMotorcyclesById")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MotorcycleDto> GetMotorcyclesById(string id)
    {
        _logger.LogInformation("START - Getting motorcycle with ID: {Id}", id);

        var motorcycle = _repository.GetMotorcyclesById(id);
        if (motorcycle != null)
        {
            _logger.LogInformation("END - Getting motorcycle with ID: {Id}", id);
            return Ok(_mapper.Map<MotorcycleDto>(motorcycle));
        }
        _logger.LogInformation("END - Getting motorcycle with ID: {Id} - NO RECORDS", id);
        return NotFound();
    }

    /// <summary>
    /// Creates a new motorcycle
    /// </summary>
    /// <param name="command">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleCreateDto>> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
    {
        _logger.LogInformation("Creating motorcycle with plate: {LicensePlate}", command.LicensePlate);

        var result = await _mediator.Send(command);

        if (result.Year.Equals(2024))
        {
            Notification notification = new Notification
            {
                DeliveryPersonId = "System",
                MotorcycleId = result.Id,
                Message = "New motorcycle from year 2024 added.",
                IsProcessed = false
            };

            var notificationMap = _mapper.Map<NotificationDto>(notification);
            notification.Evento = "NotificationPublished";
            _rabbitMqClient.SendNewNotification(notificationMap);
        }

        return CreatedAtAction(nameof(GetMotorcyclesById), new { id = result.Id }, result);
    }

    [HttpDelete("DeleteMotorcycle")]
    /// <summary>
    /// Delete a motorcycle
    /// </summary>
    /// <param name="Motorcycle">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpPost("{id}", Name = "DeleteMotorcycle")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleDto>> DeleteMotorcycle(string Id)
    {
        try
        {
            var existingMotorcycle = _repository.GetByIdAsync(Id);
            if (existingMotorcycle == null)
            {
                _logger.LogWarning("Motorcycle with ID: {Id} not found for deletion", Id);
                return NotFound();
            }
            _repository.DeleteAsync(existingMotorcycle.Result);
            _logger.LogInformation("Motorcycle with ID: {Id} deleted successfully", Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of motorcycle with ID: {Id}", Id);
            return StatusCode(500, "Internal server error");
        }

        return Ok();
    }

    [HttpPut("{id}", Name = "UpdateMotorcycle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleDto>> UpdateMotorcycle(Motorcycle Motorcycle)
    {
        try
        {
            var motorcycle = _mapper.Map<Motorcycle>(Motorcycle);
            _repository.UpdateAsync(motorcycle);
        }
        catch (Exception)
        {

            throw;
        }


        return Ok();
    }
}
