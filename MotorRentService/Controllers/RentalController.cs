#region Mainetence
/*
Comment: Created Mainetence Region, logs and try catch.
Created: 08/31/2024 15:00
Author:  Gabriel MS
*/
#endregion
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using NuGet.Protocol.Core.Types;

namespace MotorRentService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RentalController : Controller
{
    private readonly IRentalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<RentalController> _logger;

    public RentalController(IRentalRepository repository, IMapper mapper, ILogger<RentalController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new rental
    /// </summary>
    /// <param name="rentalCreateDto">Rental creation data</param>
    /// <returns>Created rental</returns>
    [HttpPost("CreateRental")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RentalDto> CreateRental(RentalCreateDto rentalCreateDto)
    {
        try
        {
            _logger.LogInformation("START - Creating rental for DeliveryPerson: {DeliveryPersonId}, Motorcycle: {MotorcycleId}", 
                rentalCreateDto.DeliveryPersonId, rentalCreateDto.MotorcycleId);

            var rental = _mapper.Map<Rental>(rentalCreateDto);
            _repository.CreateRental(rental);

            var rentalDto = _mapper.Map<RentalDto>(rental);

            _logger.LogInformation("END - Rental created successfully with ID: {RentalId}", rentalDto.Id);
            return CreatedAtRoute(nameof(GetRentalById), new { id = rentalDto.Id }, rentalDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Creating rental for DeliveryPerson: {DeliveryPersonId}, Motorcycle: {MotorcycleId}", 
                rentalCreateDto.DeliveryPersonId, rentalCreateDto.MotorcycleId);

            Error response = new Error
            {
                Message = "Dados inválidos"
            };

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Gets a rental by ID
    /// </summary>
    /// <param name="id">Rental ID</param>
    /// <returns>Rental details</returns>
    [HttpGet("{id}", Name = "GetRentalById")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RentalDto> GetRentalById(string id)
    {
        try
        {
            _logger.LogInformation("START - Getting rental with ID: {RentalId}", id);

            var rental = _repository.GetRentalById(id);
            if (rental != null)
            {
                _logger.LogInformation("END - Rental found with ID: {RentalId}", id);
                return Ok(_mapper.Map<RentalDto>(rental));
            }

            _logger.LogInformation("END - Rental not found with ID: {RentalId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Getting rental with ID: {RentalId}", id);

            Error response = new Error
            {
                Message = "Dados inválidos"
            };

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Returns a rental
    /// </summary>
    /// <param name="rentalCreateDto">Rental return data</param>
    /// <returns>Success message</returns>
    [HttpPut("ReturnRental")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ReturnRental(RentalCreateDto rentalCreateDto)
    {
        try
        {
            _logger.LogInformation("START - Returning rental for DeliveryPerson: {DeliveryPersonId}, Motorcycle: {MotorcycleId}", 
                rentalCreateDto.DeliveryPersonId, rentalCreateDto.MotorcycleId);

            var rental = _mapper.Map<Rental>(rentalCreateDto);
            _repository.ReturnRental(rental);

            _logger.LogInformation("END - Rental returned successfully for DeliveryPerson: {DeliveryPersonId}", 
                rentalCreateDto.DeliveryPersonId);
            return Ok("Locação devolvida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Returning rental for DeliveryPerson: {DeliveryPersonId}, Motorcycle: {MotorcycleId}", 
                rentalCreateDto.DeliveryPersonId, rentalCreateDto.MotorcycleId);

            Error response = new Error
            {
                Message = "Dados inválidos"
            };

            return BadRequest(response);
        }
    }

}

