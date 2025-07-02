using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface IProduct
    {
        public void Create(Product product);
        public List<Product> GetAll();
        public void Update(int id, Product product);
        public void Delete(int id);
        public Product GetById(int id);

    }
}
