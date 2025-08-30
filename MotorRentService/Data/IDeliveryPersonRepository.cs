using MotorRentService.Models;

namespace MotorRentService.Data;

public interface IDeliveryPersonRepository
{
    void SaveChanges();

    DeliveryPerson GetDeliveryPersonByCNPJNumber(string cnpj);

    void CreateDeliveryPerson(DeliveryPerson deliveryPerson);

    void UpdateCNHImage(DeliveryPerson deliveryPerson);
}

