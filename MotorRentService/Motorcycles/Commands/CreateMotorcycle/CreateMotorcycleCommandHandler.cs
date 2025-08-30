using AutoMapper;
using MediatR;
using MotorRentService.Data;
using MotorRentService.Dtos;
using MotorRentService.Events;
using MotorRentService.Models;
using MotorRentService.Services;
using System.ComponentModel.DataAnnotations;

namespace MotorRentService.Motorcycles.Commands.CreateMotorcycle;

public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, MotorcycleDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;

    public CreateMotorcycleCommandHandler(
        IMotorcycleRepository motorcycleRepository,
        IEventPublisher eventPublisher,
        IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _eventPublisher = eventPublisher;
        _mapper = mapper;
    }

    public async Task<MotorcycleDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        // Check if license plate already exists
        if (await _motorcycleRepository.ExistsByLicensePlateAsync(request.LicensePlate, cancellationToken))
        {
            throw new ValidationException($"{nameof(request.LicensePlate)} License plate already exists.");
        }

        // Create motorcycle entity
        var motorcycle = string.IsNullOrWhiteSpace(request.Id)
            ? new Motorcycle(request.Year, request.Model, request.LicensePlate)
            : new Motorcycle(request.Id, request.Year, request.Model, request.LicensePlate);

        // Save to repository
        await _motorcycleRepository.AddAsync(motorcycle, cancellationToken);

        // Publish domain event
        var motorcycleCreatedEvent = new MotorcycleCreatedEvent(
            motorcycle.Id,
            motorcycle.Year,
            motorcycle.Model,
            motorcycle.LicensePlate);

        await _eventPublisher.PublishAsync(motorcycleCreatedEvent, cancellationToken);

        // Return DTO
        return _mapper.Map<MotorcycleDto>(motorcycle);
    }
}

