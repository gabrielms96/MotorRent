#region Mainetence
/*
Comment: Roles Registration motorcycle, delete and update motorcycles, .
Created: 10/02/2025
Author:  Gabriel MS
*/
#endregion

using System.ComponentModel.DataAnnotations;
using MotorRentService.Models.Enums;

namespace MotorRentService.Models;

public class Motorcycle
{
    public Motorcycle() 
    { 
        Status = MotorcycleStatus.Available;
        CreatedAt = DateTime.UtcNow;
    }
    
    public Motorcycle(int year, string model, string licensePlate)
    {
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
        Status = MotorcycleStatus.Available;
        CreatedAt = DateTime.UtcNow;
    }

    public Motorcycle(string id, int year, string model, string licensePlate)
    {
        Id = id;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
        Status = MotorcycleStatus.Available;
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public string Id { get; set; }

    public string LicensePlate { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

    /// <summary>
    /// Motorcycle status
    /// </summary>
    public MotorcycleStatus Status { get; set; } = MotorcycleStatus.Available;

    /// <summary>
    /// Create date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
