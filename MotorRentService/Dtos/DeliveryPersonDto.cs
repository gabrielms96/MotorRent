using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorRentService.Dtos;

public class DeliveryPersonDto
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string CNPJ { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string CNHNumber { get; set; }

    [Required]
    public string CNHType { get; set; }

    public string? CNHImagePath { get; set; }
}

