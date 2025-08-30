using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorRentService.Dtos
{
    public class MotorcycleCreateDto
    {
        [Required]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }
        
        [Required]
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
        
        [Required]
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        
        [Required]
        [JsonPropertyName("modelo")]
        public string Model { get; set; }
    }
}