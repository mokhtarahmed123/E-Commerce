using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface IorderDetails
    {
        public void Create(OrderDetailes order);
        public List<OrderDetailes> GetAll();
        public void Update(int id, OrderDetailes Category);
        public void Delete(int id);
        public OrderDetailes GetById(int id);


    }
}
