using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface IPayment
    {
        public void Create(Payment Payment);
        public List<Payment> GetAll();
        public void Update(int id, Payment Payment);
        public void Delete(int id);
        public Payment GetById(int id);

    }
}
