using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontEndMotorRent.Models;

public class DeliveryPersonDto
{
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [Display(Name = "Nome")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CNPJ é obrigatório")]
    [Display(Name = "CNPJ")]
    public string CNPJ { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    [Display(Name = "Data de Nascimento")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Número da CNH é obrigatório")]
    [Display(Name = "Número da CNH")]
    public string CNHNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Tipo da CNH é obrigatório")]
    [Display(Name = "Tipo da CNH")]
    public string CNHType { get; set; } = string.Empty;
    
    [Display(Name = "Imagem da CNH")]
    public string? CNHImagePath { get; set; }
}

public class DeliveryPersonCreateDto
{
    [Required(ErrorMessage = "Identificador é obrigatório")]
    [JsonPropertyName("identificador")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nome é obrigatório")]
    [JsonPropertyName("nome")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CNPJ é obrigatório")]
    [JsonPropertyName("cnpj")]
    public string CNPJ { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    [JsonPropertyName("data_nascimento")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Número da CNH é obrigatório")]
    [JsonPropertyName("numero_cnh")]
    public string CNHNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da CNH é obrigatório")]
    [JsonPropertyName("tipo_cnh")]
    public string CNHType { get; set; } = string.Empty;

    [JsonPropertyName("imagem_cnh")]
    public string? CNHImagePath { get; set; }
} 