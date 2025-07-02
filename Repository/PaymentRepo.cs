using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class PaymentRepo : IPayment
    {
        E_CommerceContext context;
        public PaymentRepo(E_CommerceContext _context)
        {
            context = _context;
        }
        public void Create(Payment Payment)
        {
            context.Add(Payment);
            Save();
        }

        public void Delete(int id)
        {
            Payment payment = context.payments.FirstOrDefault(x => x.Id == id);
            context.payments.Remove(payment);
            Save();
        }

        public List<Payment> GetAll()
        {
            return context.payments.ToList();

        }

        public Payment GetById(int id)
        {
            return context.payments.FirstOrDefault(i => i.Id == id);
        }

        public void Update(int id, Payment Payment)
        {
            context.Update(Payment);
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
