using E_Commerce.DB;
using E_Commerce.Models;
using System.Xml.Linq;

namespace E_Commerce.Repository
{
    public class OrederDetailsRepo :IorderDetails
    {
        E_CommerceContext context;
        public OrederDetailsRepo(E_CommerceContext _Context)
        {
            context = _Context;
        }
        public void Create(OrderDetailes order)
        {
            context.Add(order);
        }

        public void Delete(int id)
        {
            OrderDetailes order = context.OrderDetailes.FirstOrDefault(o => o.Id == id);
            context.OrderDetailes.Remove(order);
            Save();
        }

        public List<OrderDetailes> GetAll()
        {
            return context.OrderDetailes.ToList();
        }

        public OrderDetailes GetById(int id)
        {
          return  context.OrderDetailes.FirstOrDefault(o => o.Id == id);
        }

        public void Update(int id, OrderDetailes OrderDetailes)
        {
           context.Update(OrderDetailes);
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
