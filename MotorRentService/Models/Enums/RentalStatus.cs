namespace MotorRentService.Models.Enums;

/// <summary>
/// Status da locação
/// </summary>
public enum RentalStatus
{
    /// <summary>
    /// Locação ativa - em andamento
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// Locação finalizada - devolvida no prazo
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Locação cancelada
    /// </summary>
    Cancelled = 3,
    
    /// <summary>
    /// Locação em atraso - passou da data prevista
    /// </summary>
    Overdue = 4
}

