using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorRentService.Dtos;

public class DeliveryPersonCreateDto
{
    [Required]
    [JsonPropertyName("identificador")]
    public string Id { get; set; }

    [Required]
    [JsonPropertyName("nome")]
    public string Name { get; set; }
    
    [Required]
    [JsonPropertyName("cnpj")]
    public string CNPJ { get; set; }

    [Required]
    [JsonPropertyName("data_nascimento")]
    public DateTime BirthDate { get; set; }

    [Required]
    [JsonPropertyName("numero_cnh")]
    public string CNHNumber { get; set; }

    [Required]
    [JsonPropertyName("tipo_cnh")]
    public string CNHType { get; set; }

    [JsonPropertyName("imagem_cnh")]
    public string? CNHImagePath { get; set; }
}

