using FluentValidation;

namespace MotorRentService.Motorcycles.Commands.CreateMotorcycle;
    public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        public CreateMotorcycleCommandValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(0)
                .WithMessage("Year must be greater than zero.");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required.")
                .MaximumLength(100)
                .WithMessage("Model must not exceed 100 characters.");

            RuleFor(x => x.LicensePlate)
                .NotEmpty()
                .WithMessage("License plate is required.")
                .MaximumLength(20)
                .WithMessage("License plate must not exceed 20 characters.");
        }
    }
