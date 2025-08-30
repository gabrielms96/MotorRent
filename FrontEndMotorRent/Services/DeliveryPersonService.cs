using FrontEndMotorRent.Models;

namespace FrontEndMotorRent.Services;

public class DeliveryPersonService : ApiService
{
    public DeliveryPersonService(HttpClient httpClient, IConfiguration configuration) 
        : base(httpClient, configuration)
    {
    }

    public async Task<bool> CreateDeliveryPersonAsync(DeliveryPersonCreateDto deliveryPerson)
    {
        return await PostAsync("api/DeliveryPerson/CreateDeliveryPerson", deliveryPerson);
    }

    public async Task<DeliveryPersonDto?> GetDeliveryPersonByCNPJAsync(string cnpj)
    {
        return await GetAsync<DeliveryPersonDto>($"api/DeliveryPerson/{cnpj}");
    }

    public async Task<bool> UpdateCNHImageAsync(string cnpj, string cnhImagePath)
    {
        return await PostAsync($"api/DeliveryPerson/UpdateCNHImage/{cnpj}", cnhImagePath);
    }
} 