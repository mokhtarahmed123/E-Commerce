using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface ICategeoy
    {
        public void Create(Category Category);
        public List<Category> GetAll();
        public void Update(int id, Category Category);
        public void Delete(int id);
        public Category GetById(int id);
        public string GetByName(int id);

    }
}
