namespace MotorRentService.Services;

/// <summary>
/// Interface for validation services
/// </summary>
public interface IValidationService
{
    bool IsValidCNPJ(string cnpj);
    bool IsValidCNHNumber(string cnhNumber);
    bool IsValidCNHTypeForMotorcycle(string cnhType);
    bool IsValidAge(DateTime birthDate);
    bool IsValidBirthDate(DateTime birthDate);
    bool IsValidLicensePlate(string licensePlate);
}

