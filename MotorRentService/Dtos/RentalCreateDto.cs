using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorRentService.Dtos
{
    public class RentalCreateDto
    {
        [Required]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }
        
        [Required]
        [JsonPropertyName("entregador_id")]
        public string DeliveryPersonId { get; set; }

        [Required]
        [JsonPropertyName("moto_id")]
        public string MotorcycleId { get; set; }

        [JsonIgnore]
        public DateTime ActualDate { get; set; }

        [Required]
        [JsonPropertyName("entregador_id")]
        public DateTime StartDate { get; set; }

        [Required]
        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        [Required]
        [JsonPropertyName("data_termino")]
        public DateTime? RealEndDate { get; set; }

        [Required]
        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }
        
        [Required]
        [JsonPropertyName("plano")]
        public int IdPlan { get; set; }
        
        [Required]
        [JsonPropertyName("valor_diaria")]
        public decimal DailyRate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? FineAmount { get; set; }
        public decimal? FinalValue { get; set; }
    }
}
