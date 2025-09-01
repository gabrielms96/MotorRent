#region Mainetence
/*
Comment: Roles Registration motorcycle, delete and update motorcycles, .
Created: 08/31/2024 21:10
Author:  Gabriel MS
*/
#endregion
using Amazon.JSII.JsonModel.Api.Response;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using MotorRentService.Motorcycles.Commands.CreateMotorcycle;
using MotorRentService.RabbitMqClient;
using System.Data;

namespace MotorRentService.Controllers;

[Route("motos/")]
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


    /// <summary>
    /// Gets a motorcycle by ID
    /// </summary>
    /// <param name="id">Motorcycle ID</param>
    /// <returns>Motorcycle details</returns>
    [HttpGet("GetAllMotorcycles")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<MotorcycleDto>> GetAllMotorcycles()
    {
        try
        {
            _logger.LogInformation("START - Getting all motorcycle");

            var motorcycle = _repository.GetAllAsync();
            if (motorcycle.Result.Count() != 0)
            {
                _logger.LogInformation("END - Getting all motorcycle: {count}", motorcycle.Result.Count());
                return Ok((List<MotorcycleDto>)_mapper.Map<IEnumerable<MotorcycleDto>>(motorcycle.Result));
            }
            _logger.LogInformation("END - Getting all motorcycle: NO RECORDS");
            throw new Exception("No records found");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("ERRO - Gettin all motorcycle:{error}", ex.Message);

            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Gets a motorcycle by ID
    /// </summary>
    /// <param name="id">Motorcycle ID</param>
    /// <returns>Motorcycle details</returns>
    [HttpGet("{id}", Name = "GetMotorcyclesById")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<MotorcycleDto>> GetMotorcyclesById(string id)
    {
        try
        {
            _logger.LogInformation("START - Getting motorcycle with ID: {Id}", id);

            var motorcycle = _repository.GetMotorcyclesById(id);
            if (motorcycle.Result.Count() > 0)
            {
                _logger.LogInformation("END - Getting motorcycle with ID: {Id}", id);
                return Ok((List<MotorcycleDto>)_mapper.Map<IEnumerable<Motorcycle>>(motorcycle.Result));
            }
            _logger.LogInformation("END - Getting motorcycle with ID: {Id} - NO RECORDS", id);
            return NotFound();
        }
        catch (Exception)
        {

            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Gets a motorcycle by ID
    /// </summary>
    /// <param name="id">Motorcycle ID</param>
    /// <returns>Motorcycle details</returns>
    [HttpGet("GetMotorcyclesByLicensePlate")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<MotorcycleDto>> GetMotorcyclesByLicensePlate(string id)
    {
        try
        {
            _logger.LogInformation("START - Getting motorcycle with License Plate: {Id}", id);

            var motorcycle = _repository.GetByLicensePlateAsync(id);
            if (motorcycle.Result.Count() > 0)
            {
                _logger.LogInformation("END - Getting motorcycle with License Plate: {Id}", id);
                return Ok((List<MotorcycleDto>)_mapper.Map<IEnumerable<Motorcycle>>(motorcycle.Result));
            }
            _logger.LogInformation("END - Getting motorcycle with License Plate: {Id} - NO RECORDS", id);
            return NotFound();
        }
        catch (Exception)
        {

            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Creates a new motorcycle
    /// </summary>
    /// <param name="command">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpPost("CreateMotorcycle")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleCreateDto>> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
    {
        try
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
        catch (Exception)
        {
            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }


    /// <summary>
    /// Delete a motorcycle
    /// </summary>
    /// <param name="Motorcycle">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpDelete("{id}", Name = "DeleteMotorcycle")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleDto>> DeleteMotorcycle([FromRoute] string id)
    {
        try
        {
            var existingMotorcycle = _repository.GetByIdAsync(id);
            if (existingMotorcycle.Result == null)
            {
                _logger.LogWarning("Motorcycle with ID: {Id} not found for deletion", id);
                return NotFound();
            }

            var existRegistration = _repository.ExistMotorcycleRegistrationAsync(existingMotorcycle.Result.LicensePlate, existingMotorcycle.Result.Id);

            if (existRegistration.Result)
            {
                _logger.LogWarning("Motorcycle with License Plate: {LicensePlate} has active registrations and cannot be deleted", existingMotorcycle.Result.LicensePlate);
                return Ok("Motorcycle has active registrations and cannot be deleted.");
            }

            await _repository.DeleteAsync(existingMotorcycle.Result);
            _logger.LogInformation("Motorcycle with ID: {Id} deleted successfully", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Deleting motorcycle with ID: {Id}", id);
            
            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }

    [HttpPut("{id}/placa", Name = "UpdateMotorcycle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotorcycleDto>> UpdateMotorcycle([FromRoute]string id, string LicensePlate)
    {
        try
        {
            var motorcycle = _repository.GetByIdAsync(id);
            if (motorcycle.Result == null)
            {
                _logger.LogWarning("Motorcycle with ID: {Id} not found for update", id);
                throw new Exception($"Motorcycle with ID: {id} not found for update");
            }

            _logger.LogInformation($"START - Updating motorcycle: {motorcycle.Id}");
            Motorcycle motorcycleNewLicencePlate = _mapper.Map<Motorcycle>(motorcycle.Result);
            motorcycleNewLicencePlate.LicensePlate = LicensePlate;
            await _repository.UpdateAsync(motorcycleNewLicencePlate);
            _logger.LogInformation($"END - Updated motorcycle: {motorcycle.Id}");
            return Ok("Placa modificada com sucesso");
        }
        catch (Exception)
        {

            Error response = new Error
            {
                Message = "Dados inválidos",
            };

            return BadRequest(response);
        }
    }
}
