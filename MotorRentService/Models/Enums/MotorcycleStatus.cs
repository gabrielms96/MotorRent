namespace MotorRentService.Models.Enums;

/// <summary>
/// Status da motocicleta
/// </summary>
public enum MotorcycleStatus
{
    /// <summary>
    /// Disponível para locação
    /// </summary>
    Available = 1,
    
    /// <summary>
    /// Alugada - em uso
    /// </summary>
    Rented = 2,
    
    /// <summary>
    /// Em manutenção - indisponível
    /// </summary>
    Maintenance = 3,
    
    /// <summary>
    /// Inativa - fora de operação
    /// </summary>
    Inactive = 4
}

