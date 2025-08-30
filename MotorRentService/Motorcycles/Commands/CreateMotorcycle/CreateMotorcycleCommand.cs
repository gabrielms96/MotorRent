using MediatR;
using MotorRentService.Dtos;

namespace MotorRentService.Motorcycles.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommand : IRequest<MotorcycleDto>
    {
        public string? Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
    }
}
