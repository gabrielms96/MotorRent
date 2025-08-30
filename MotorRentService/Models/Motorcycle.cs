using System.ComponentModel.DataAnnotations;

namespace MotorRentService.Models;

public class Motorcycle
{
    public Motorcycle() { }
    public Motorcycle(int year, string model, string licensePlate)
    {
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    public Motorcycle(string id, int year, string model, string licensePlate)
    {
        Id = id;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    [Key]
    public string Id { get; set; }

    public string LicensePlate { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

}
