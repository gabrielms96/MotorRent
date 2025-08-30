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
            if (deliveryPerson == null)
            {
                throw new ArgumentNullException(nameof(deliveryPerson));
            }

            _context.DeliveryPerson.Add(deliveryPerson);
            SaveChanges();
        }

        public DeliveryPerson GetDeliveryPersonByCNPJNumber(string cnpj)
        {
            var delivereyPerson = _context.DeliveryPerson.FirstOrDefault(c => c.CNPJ == cnpj);
            if (delivereyPerson == null)
                throw new ArgumentNullException(nameof(delivereyPerson));
            return delivereyPerson;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateCNHImage(DeliveryPerson deliveryPerson)
        {
            _context.DeliveryPerson.Update(deliveryPerson);
            SaveChanges();
        }
    }
}
