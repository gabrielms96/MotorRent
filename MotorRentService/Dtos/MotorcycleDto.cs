using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorRentService.Dtos;

public class MotorcycleDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string LicensePlate { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string Model { get; set; }
}
