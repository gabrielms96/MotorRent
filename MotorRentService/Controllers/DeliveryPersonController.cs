using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using MotorRentService.RabbitMqClient;

namespace MotorRentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryPersonController : Controller
{
    private readonly IDeliveryPersonRepository _repository;
    private readonly IMapper _mapper;

    public DeliveryPersonController(IDeliveryPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost("CreateDeliveryPerson")]
    public async Task<ActionResult<DeliveryPersonDto>> CreateDeliveryPerson(DeliveryPersonCreateDto DeliveryPersonCreateDto)
    {
        var deliveryPerson = _mapper.Map<DeliveryPerson>(DeliveryPersonCreateDto);
        _repository.CreateDeliveryPerson(deliveryPerson);

        var motorcycleReadDto = _mapper.Map<MotorcycleDto>(deliveryPerson);

        return Ok();
    }

    [HttpGet("{cnpj}", Name = "GetDeliveryPersonByCNPJNumber")]
    public ActionResult<DeliveryPersonDto> GetDeliveryPersonByCNPJNumber(string cnpj)
    {
        var deliveryPerson = _repository.GetDeliveryPersonByCNPJNumber(cnpj);
        if (deliveryPerson != null)
        {
            return Ok(_mapper.Map<DeliveryPersonDto>(deliveryPerson));
        }
        return NotFound();
    }

    [HttpPost("UpdateCNHImage/{cnpj}")]
    public ActionResult UpdateCNHImage(string cnpj, [FromBody] string cnhImagePath)
    {
        var deliveryPerson = _repository.GetDeliveryPersonByCNPJNumber(cnpj);
        if (deliveryPerson == null)
        {
            return NotFound();
        }
        deliveryPerson.CNHImage = cnhImagePath;
        _repository.UpdateCNHImage(deliveryPerson);
        return NoContent();
    }
}

