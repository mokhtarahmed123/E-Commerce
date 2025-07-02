using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class ProductRepo : IProduct
    {
        E_CommerceContext context;

        public ProductRepo( E_CommerceContext Ecom )
        {
            context = Ecom;
        }
        public void Create(Product product)
        {
           context.Product.Add( product );
            Save();
        }

        public void Delete(int id)
        {
            Product product = context.Product.FirstOrDefault(i=>i.Id== id );
            context.Product.Remove( product );
            Save();
        }

        public List<Product> GetAll()
        {
            return context.Product.ToList();
        }

        public Product GetById(int id)
        {
            return context.Product.FirstOrDefault(i => i.Id == id);
        }

        public void Update(int id, Product product)
        {
            context.Product.Update(product);
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
