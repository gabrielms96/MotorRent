#region Mainetence
/*
Comment: Created Mainetence Region and correction mapping.
Created: 08/31/2024 15:00
Author: Gabriel MS
----------------------------------------------------------------------
Comment: Validate CNH image and CNPJ method created, logging and error 
         handling added, and repository method for updating CNH image.
Created: 08/31/2024 18:31
Author: Gabriel MS
*/
#endregion
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Models;
using MotorRentService.RabbitMqClient;
using System.Linq.Expressions;
using System.Text.Json;

namespace MotorRentService.Controllers;

[Route("entregadores/")]
[ApiController]
[Produces("application/json")]
public class DeliveryPersonController : Controller
{
    private readonly IDeliveryPersonRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeliveryPersonController> _logger;

    public DeliveryPersonController(IDeliveryPersonRepository repository, IMapper mapper, ILogger<DeliveryPersonController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }


    /// <summary>
    /// Create a Delivery Person
    /// </summary>
    /// <param name="Motorcycle">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpPost()]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<DeliveryPersonDto>> CreateDeliveryPerson(DeliveryPersonCreateDto DeliveryPersonCreateDto)
    {
        try
        {
            _logger.LogInformation("START - Creating a new delivery person with CNPJ: {CNPJ}", DeliveryPersonCreateDto.CNPJ);
            var deliveryPerson = _mapper.Map<DeliveryPerson>(DeliveryPersonCreateDto);
            _repository.CreateDeliveryPerson(deliveryPerson);

            var deliveryPersonDto = _mapper.Map<DeliveryPersonDto>(deliveryPerson);

            _logger.LogInformation("END - Created a new delivery person with CNPJ: {CNPJ}", DeliveryPersonCreateDto.CNPJ);

            return Ok(deliveryPersonDto);
        }
        catch (Exception ex)
        {
            Error response = new Error
            {
                Message = "Dados inválidos",
            };
            _logger.LogError("ERROR - Creating a new delivery person with CNPJ: {CNPJ} - {ErrorMessage}", DeliveryPersonCreateDto.CNPJ, ex.Message);

            return Json(response);
        }
    }

    [HttpGet("{cnpj}", Name = "GetDeliveryPersonByCNPJNumber")]
    public ActionResult<DeliveryPersonDto> GetDeliveryPersonByCNPJNumber(string cnpj)
    {
        try
        {
            _logger.LogError("START - Getting delivery person by CNPJ");
            var deliveryPerson = _repository.GetDeliveryPersonByCNPJNumber(cnpj);
            if (deliveryPerson != null)
            {
                _logger.LogError("END - Getting delivery person by CNPJ");
                return Ok(_mapper.Map<DeliveryPersonDto>(deliveryPerson));
            }
            _logger.LogError("END - Getting delivery person by CNPJ");
            return NotFound();
        }
        catch (Exception ex)
        {
            Error response = new Error
            {
                Message = ex.Message,
            };
            _logger.LogError("ERROR - Getting delivery person by CNPJ: {cnpj} - {ErrorMessage}", cnpj, ex.Message);

            return Json(response);
        }
    }

    /// <summary>
    /// Send CNH image to S3 and update delivery person
    /// </summary>
    /// <param name="Motorcycle">Motorcycle creation data</param>
    /// <returns>Created motorcycle</returns>
    [HttpPost("{cnpj}/cnh")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    []
    public ActionResult UpdateCNHImage(string cnpj, string cnhImagePath)
    {
        try
        {
            _logger.LogError("START - Updating CNH image for delivery person by CNPJ");
            _logger.LogError("START - Validating CNH image and CNPJ");

            bool validate = ValidatePictureCnpj(cnhImagePath, cnpj);
            var deliveryPerson = _repository.GetDeliveryPersonByCNPJNumber(cnpj);
            if (deliveryPerson == null || !validate)
            {
                _logger.LogError("END - Validating CNH image and CNPJ");

                return NotFound();
            }
            _logger.LogError("END - Validating CNH image and CNPJ");

            deliveryPerson.CNHImage = cnhImagePath;
            S3Uploader s3Uploader = new S3Uploader();
            s3Uploader.UploadFoto(cnhImagePath, cnpj);
            _repository.UpdateCNHImage(deliveryPerson);
            return NoContent();
        }
        catch (Exception ex)
        {

            Error response = new Error
            {
                Message = "Dados inválidos",
            };
            _logger.LogError("ERROR - Getting delivery person by CNPJ: {cnpj} - {ErrorMessage}", cnpj, ex.Message);

            return Json(response);
        }
    }

    private bool ValidatePictureCnpj(string cnhImagePath, string cnpj)
    {
        if (string.IsNullOrEmpty(cnhImagePath) || string.IsNullOrEmpty(cnpj))
        {
            _logger.LogError("END - Validating CNH image and CNPJ");
            return false;
        }

        if (!System.IO.File.Exists(cnhImagePath))
        {
            _logger.LogError("END - Validating CNH image and CNPJ");
            return false;
        }

        string[] extensionsPermit = [".png", ".bmp"];
        string extension = Path.GetExtension(cnhImagePath).ToLower();

        if (!extensionsPermit.Contains(extension))
        {
            _logger.LogError("END - Validating CNH image and CNPJ");
            return false;
        }

        return true;
    }

}

