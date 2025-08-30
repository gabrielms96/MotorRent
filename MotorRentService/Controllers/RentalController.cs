using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using NuGet.Protocol.Core.Types;

namespace MotorRentService.Controllers;

[Route("api/teste/[controller]")]
[ApiController]
public class RentalController : Controller
{
    private readonly IRentalRepository _repository;
    private readonly IMapper _mapper;

    public RentalController(IRentalRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost("CreateRental")]
    public async Task<ActionResult<RentalDto>> CreateRental(RentalCreateDto rentalCreateDto)
    {
        var rental = _mapper.Map<Rental>(rentalCreateDto);
        _repository.CreateRental(rental);

        var motorcycleReadDto = _mapper.Map<RentalDto>(rental);

        return CreatedAtRoute(nameof(GetRentalById), new { motorcycleReadDto.Id }, motorcycleReadDto);
    }

    [HttpGet("{id}", Name = "GetRentalById")]
    public async Task<ActionResult<RentalDto>> GetRentalById(string rentalId)
    {
        var rental = _repository.GetRentalById(rentalId);
        if (rental != null)
        {
            return Ok(_mapper.Map<RentalDto>(rental));
        }

        return NotFound();
    }

    [HttpPut("ReturnRental")]
    public async Task<ActionResult<RentalDto>> ReturnRental(RentalCreateDto rentalCreateDto)
    {
        var rental = _mapper.Map<Rental>(rentalCreateDto);
        _repository.ReturnRental(rental);
  

        return Ok();
    }

}

