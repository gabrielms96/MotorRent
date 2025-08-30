using FrontEndMotorRent.Models;

namespace FrontEndMotorRent.Services;

public class MotorcycleService : ApiService
{
    public MotorcycleService(HttpClient httpClient, IConfiguration configuration) 
        : base(httpClient, configuration)
    {
    }

    public async Task<IEnumerable<MotorcycleDto>?> GetAllMotorcyclesAsync()
    {
        return await GetAsync<IEnumerable<MotorcycleDto>>("api/Motorcycle/GetAllMotorcycles");
    }

    public async Task<MotorcycleDto?> GetMotorcycleByIdAsync(string id)
    {
        return await GetAsync<MotorcycleDto>($"api/Motorcycle/{id}");
    }

    public async Task<bool> CreateMotorcycleAsync(MotorcycleCreateDto motorcycle)
    {
        return await PostAsync("api/Motorcycle", motorcycle);
    }

    public async Task<bool> UpdateMotorcycleAsync(string id, MotorcycleDto motorcycle)
    {
        return await PutAsync($"api/Motorcycle/{id}", motorcycle);
    }

    public async Task<bool> DeleteMotorcycleAsync(string id)
    {
        return await DeleteAsync($"api/Motorcycle/{id}");
    }
} 