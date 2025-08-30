using Microsoft.OpenApi.Any;
using System.ComponentModel.DataAnnotations;

namespace MotorRentService.Models
{
    public class DeliveryPerson
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CNPJ { get; set; }

        public DateTime BirthDate { get; set; }

        public string CNHNumber { get; set; }

        public string CNHType { get; set; }

        public string? CNHImage { get; set; }

    }
}
