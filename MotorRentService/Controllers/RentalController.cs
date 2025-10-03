#region Mainetence
/*
Comment: Created Mainetence Region, logs and try catch.
Created: 08/31/2024 15:00
Author:  Gabriel MS

Comment: Enhanced with business rules validation.
Updated: 10/03/2025
Author:  Gabriel MS
*/
#endregion
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using MotorRentService.Models.Enums;
using MotorRentService.Services;
using NuGet.Protocol.Core.Types;

namespace MotorRentService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RentalController : Controller
{
    private readonly IRentalRepository _repository;
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RentalController> _logger;
    private readonly IValidationService _validationService;
    private readonly IMotorcycleAvailabilityService _motorcycleAvailabilityService;
    private readonly IRentalCalculationService _rentalCalculationService;

    public RentalController(
        IRentalRepository repository,
        IDeliveryPersonRepository deliveryPersonRepository,
        IMapper mapper,
        ILogger<RentalController> logger,
        IValidationService validationService,
        IMotorcycleAvailabilityService motorcycleAvailabilityService,
        IRentalCalculationService rentalCalculationService)
    {
        _repository = repository;
        _deliveryPersonRepository = deliveryPersonRepository;
        _mapper = mapper;
        _logger = logger;
        _validationService = validationService;
        _motorcycleAvailabilityService = motorcycleAvailabilityService;
        _rentalCalculationService = rentalCalculationService;
    }

    /// <summary>
    /// Creates a new rental with complete business rules validation
    /// </summary>
    /// <param name="rentalCreateDto">Rental creation data</param>
    /// <returns>Created rental</returns>
    [HttpPost("CreateRental")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalDto>> CreateRental(RentalCreateDto rentalCreateDto)
    {
        try
        {
            _logger.LogInformation("START - Creating rental for DeliveryPerson: {DeliveryPersonId}, Motorcycle: {MotorcycleId}", 
                rentalCreateDto.DeliveryPersonId, rentalCreateDto.MotorcycleId);

            if (string.IsNullOrWhiteSpace(rentalCreateDto.DeliveryPersonId) || 
                string.IsNullOrWhiteSpace(rentalCreateDto.MotorcycleId))
            {
                return BadRequest(new Error { Message = "Motorcycle ID and Delivery Person ID must be filled in." });
            }

            var deliveryPerson = _deliveryPersonRepository.GetDeliveryPersonByCNPJNumber(rentalCreateDto.DeliveryPersonId);
            if (deliveryPerson == null)
            {
                return BadRequest(new Error { Message = "Delivery Person not found." });
            }

            if (!_validationService.IsValidCNHTypeForMotorcycle(deliveryPerson.CNHType))
            {
                return BadRequest(new Error { Message = "CNH type must be A or AB." });
            }

            if (!_validationService.IsValidAge(deliveryPerson.BirthDate))
            {
                return BadRequest(new Error { Message = "Delivery Person must be at least 18 years old." });
            }

            if (!await _motorcycleAvailabilityService.IsMotorcycleAvailableAsync(rentalCreateDto.MotorcycleId))
            {
                return BadRequest(new Error { Message = "Motorcycle unavailable to rent." });
            }

            var endDate = await _rentalCalculationService.CalculateEndDateAsync(rentalCreateDto.IdPlan, rentalCreateDto.StartDate);
            if (!endDate.HasValue)
            {
                return BadRequest(new Error { Message = "Invalid Rental plan." });
            }

            if (!_rentalCalculationService.ValidateRentalDates(rentalCreateDto.StartDate, endDate.Value))
            {
                return BadRequest(new Error { Message = "Invalid Rental dates." });
            }

            if (!await _motorcycleAvailabilityService.IsMotorcycleAvailableInPeriodAsync(
                rentalCreateDto.MotorcycleId, rentalCreateDto.StartDate, endDate.Value))
            {
                return BadRequest(new Error { Message = "Motorcycle unavailable to rent." });
            }

            var totalCost = await _rentalCalculationService.CalculateTotalCostAsync(
                rentalCreateDto.IdPlan, rentalCreateDto.StartDate, endDate.Value);

            var rental = _mapper.Map<Rental>(rentalCreateDto);
            rental.Id = string.IsNullOrWhiteSpace(rental.Id) ? Guid.NewGuid().ToString() : rental.Id;
            rental.ExpectedEndDate = endDate.Value;
            rental.TotalCost = totalCost;
            rental.Status = RentalStatus.Active;
            rental.CreatedAt = DateTime.UtcNow;

            await _motorcycleAvailabilityService.MarkMotorcycleAsRentedAsync(rentalCreateDto.MotorcycleId);

            await _repository.CreateRentalAsync(rental);

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
                Message = "Erro interno do servidor. Tente novamente."
            };

            return StatusCode(500, response);
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
    public async Task<ActionResult<RentalDto>> GetRentalById(string id)
    {
        try
        {
            _logger.LogInformation("START - Getting rental with ID: {RentalId}", id);

            var rental = await _repository.GetRentalByIdAsync(id);
            if (rental != null)
            {
                _logger.LogInformation("END - Rental found with ID: {RentalId}", id);
                return Ok(_mapper.Map<RentalDto>(rental));
            }

            _logger.LogInformation("END - Rental not found with ID: {RentalId}", id);
            return NotFound(new Error { Message = "Rental not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Getting rental with ID: {RentalId}", id);

            Error response = new Error
            {
                Message = "Server error."
            };

            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Returns a rental with complete fine calculation
    /// </summary>
    /// <param name="rentalId">Rental ID</param>
    /// <param name="returnDate">Return date</param>
    /// <returns>Return details with calculated values</returns>
    [HttpPut("ReturnRental/{rentalId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ReturnRental(string rentalId, [FromBody] DateTime returnDate)
    {
        try
        {
            _logger.LogInformation("START - Returning rental: {RentalId} on {ReturnDate}", rentalId, returnDate);

            var rental = await _repository.GetRentalByIdAsync(rentalId);
            if (rental == null)
            {
                return NotFound(new Error { Message = "Rental not found." });
            }

            if (rental.Status != RentalStatus.Active)
            {
                return BadRequest(new Error { Message = "Rental is not active." });
            }

            if (returnDate < rental.StartDate)
            {
                return BadRequest(new Error { Message = "Return Date must be after Start Date." });
            }

            var earlyReturnFine = await _rentalCalculationService.CalculateEarlyReturnFineAsync(rental, returnDate);
            var lateReturnFine = await _rentalCalculationService.CalculateLateReturnFineAsync(rental, returnDate);
            var finalValue = await _rentalCalculationService.CalculateFinalValueAsync(rental, returnDate);

            // 5. Atualizar dados da locação
            rental.ReturnDate = returnDate;
            rental.RealEndDate = returnDate;
            rental.FineAmount = earlyReturnFine + lateReturnFine;
            rental.FinalValue = finalValue;
            rental.Status = RentalStatus.Completed;
            rental.UpdatedAt = DateTime.UtcNow;

            // 6. Marcar motocicleta como disponível
            await _motorcycleAvailabilityService.MarkMotorcycleAsAvailableAsync(rental.MotorcycleId);

            // 7. Salvar alterações
            await _repository.ReturnRentalAsync(rental);

            // 8. Preparar resposta
            var response = new
            {
                Message = "Locação devolvida com sucesso",
                RentalId = rental.Id,
                ReturnDate = returnDate,
                ExpectedEndDate = rental.ExpectedEndDate,
                TotalCost = rental.TotalCost,
                EarlyReturnFine = earlyReturnFine,
                LateReturnFine = lateReturnFine,
                TotalFine = rental.FineAmount,
                FinalValue = rental.FinalValue,
                Status = rental.Status.ToString()
            };

            _logger.LogInformation("END - Rental {RentalId} returned successfully. Final value: {FinalValue}", 
                rentalId, finalValue);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR - Returning rental: {RentalId}", rentalId);

            Error response = new Error
            {
                Message = "Erro interno do servidor. Tente novamente."
            };

            return StatusCode(500, response);
        }
    }

}

