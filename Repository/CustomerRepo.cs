using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class CustomerRepo : ICustomer
    {
        E_CommerceContext context;
       
        public CustomerRepo(E_CommerceContext e_CommerceContext)
        {
            context = e_CommerceContext;
        }
        public void Create(Customer customer)
        {
            context.Customers.Add(customer);
            Save();
        }

        public void Delete(int id)
        {
            Customer c =  context.Customers.FirstOrDefault(x => x.Id == id);
            context.Customers.Remove(c);
            Save();
        }

        public List<Customer> GetAll()
        {
            return context.Customers.ToList();
        }

        public Customer GetById(int id)
        {
            return context.Customers.FirstOrDefault(i=>i.Id == id);
        }

        public void Update(int id, Customer customer )
        {
            context.Update(customer);
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
