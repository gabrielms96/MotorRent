using FrontEndMotorRent.Models;

namespace FrontEndMotorRent.Services;

public class RentalService : ApiService
{
    public RentalService(HttpClient httpClient, IConfiguration configuration) 
        : base(httpClient, configuration)
    {
    }

    public async Task<bool> CreateRentalAsync(RentalCreateDto rental)
    {
        return await PostAsync("api/Rental/CreateRental", rental);
    }

    public async Task<RentalDto?> GetRentalByIdAsync(string id)
    {
        return await GetAsync<RentalDto>($"api/Rental/{id}");
    }

    public async Task<bool> ReturnRentalAsync(RentalCreateDto rental)
    {
        return await PutAsync("api/Rental/ReturnRental", rental);
    }

    public async Task<T?> ProcessRentalReturnAsync<T>(string rentalId, DateTime returnDate)
    {
        return await PutAsync<object, T>($"api/Rental/ReturnRental/{rentalId}", returnDate);
    }
} 