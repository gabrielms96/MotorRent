using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontEndMotorRent.Models;

public class MotorcycleDto
{
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "Placa é obrigatória")]
    [Display(Name = "Placa")]
    public string LicensePlate { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ano é obrigatório")]
    [Display(Name = "Ano")]
    [Range(1900, 2100, ErrorMessage = "Ano deve estar entre 1900 e 2100")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Modelo é obrigatório")]
    [Display(Name = "Modelo")]
    public string Model { get; set; } = string.Empty;
}

public class MotorcycleCreateDto
{
    [Required(ErrorMessage = "Identificador é obrigatório")]
    [JsonPropertyName("identificador")]
    public string Id { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Placa é obrigatória")]
    [JsonPropertyName("placa")]
    public string LicensePlate { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ano é obrigatório")]
    [JsonPropertyName("ano")]
    [Range(1900, 2100, ErrorMessage = "Ano deve estar entre 1900 e 2100")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Modelo é obrigatório")]
    [JsonPropertyName("modelo")]
    public string Model { get; set; } = string.Empty;
} 