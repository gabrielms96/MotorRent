using MotorRentService.Models;

namespace MotorRentService.Data
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private AppDbContext _context;

        public DeliveryPersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateDeliveryPerson(DeliveryPerson deliveryPerson)
        {
            try
            {
                if (deliveryPerson == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPerson));
                }
                _context.DeliveryPerson.Add(deliveryPerson);
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public DeliveryPerson GetDeliveryPersonByCNPJNumber(string cnpj)
        {
            try
            {
                var delivereyPerson = _context.DeliveryPerson.FirstOrDefault(c => c.CNPJ == cnpj);
                if (delivereyPerson == null)
                    throw new ("Delivery Person not found");
                return delivereyPerson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void UpdateCNHImage(DeliveryPerson deliveryPerson)
        {
            try
            {
                _context.DeliveryPerson.Update(deliveryPerson);
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
