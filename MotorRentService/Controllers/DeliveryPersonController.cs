#region Mainetence
/*
Comment: Created Mainetence Region and correction mapping.
Created: 08/31/2024 15:00
Author: Gabriel MS
*/
#endregion
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

        var deliveryPersonDto = _mapper.Map<DeliveryPersonDto>(deliveryPerson);

        return Ok(deliveryPersonDto);
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

