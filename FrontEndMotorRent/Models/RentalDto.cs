using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontEndMotorRent.Models;

public class RentalDto
{
    public string? Id { get; set; }
    public string? DeliveryPersonId { get; set; }
    public string? MotorcycleId { get; set; }
    public DateTime ActualDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? RealEndDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int IdPlan { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? FineAmount { get; set; }
    public decimal? FinalValue { get; set; }
}

public class RentalCreateDto
{
    [Required(ErrorMessage = "Identificador é obrigatório")]
    [JsonPropertyName("identificador")]
    public string Id { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ID do entregador é obrigatório")]
    [JsonPropertyName("entregador_id")]
    public string DeliveryPersonId { get; set; } = string.Empty;

    [Required(ErrorMessage = "ID da moto é obrigatório")]
    [JsonPropertyName("moto_id")]
    public string MotorcycleId { get; set; } = string.Empty;

    [JsonIgnore]
    public DateTime ActualDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Data de início é obrigatória")]
    [JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Data prevista de término é obrigatória")]
    [JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; set; }

    [JsonPropertyName("data_termino")]
    public DateTime? RealEndDate { get; set; }

    [JsonPropertyName("data_devolucao")]
    public DateTime? ReturnDate { get; set; }
    
    [Required(ErrorMessage = "Plano é obrigatório")]
    [JsonPropertyName("plano")]
    public int IdPlan { get; set; }
    
    [Required(ErrorMessage = "Valor da diária é obrigatório")]
    [JsonPropertyName("valor_diaria")]
    public decimal DailyRate { get; set; }
    
    public decimal TotalCost { get; set; }
    public decimal? FineAmount { get; set; }
    public decimal? FinalValue { get; set; }
}

public class RentalPlanDto
{
    public int Id { get; set; }
    
    [Display(Name = "Nome do Plano")]
    public string PlanName { get; set; } = string.Empty;

    [Display(Name = "Duração (dias)")]
    public int DurationDays { get; set; }

    [Display(Name = "Valor Diário")]
    public decimal DailyRate { get; set; }

    [Display(Name = "Taxa de Multa")]
    public decimal FineRate { get; set; }
} 