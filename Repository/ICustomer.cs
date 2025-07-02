using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface ICustomer
    {
        public void Create(Customer customer );
        public List<Customer> GetAll();
        public void Update(int id, Customer newCustomer);
        public void Delete(int id);
        public Customer GetById(int id);
       

    }
}
