using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class CategoryRepo : ICategeoy
    {
        private readonly E_CommerceContext context;

        public CategoryRepo(E_CommerceContext _context)
        {
            context = _context;
        }

        public void Create(Category Category)
        {
            context.Add(Category);
            Save();
        }

        public void Delete(int id)
        {
            Category category = context.Categorys.FirstOrDefault(i => i.Id == id);
            context.Remove(category);
            Save();
        }

        public List<Category> GetAll()
        {
            return context.Categorys.ToList();
        }

        public Category GetById(int id)
        {
            return context.Categorys.FirstOrDefault(i => i.Id == id);
        }

        public void Update(int id, Category updatedCategory)
        {
            Category existing = context.Categorys.FirstOrDefault(c => c.Id == id);
            if (existing != null)
            {
                existing.Name = updatedCategory.Name;
                Save();
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public string GetByName(int id)
        {
            Category category = context.Categorys.FirstOrDefault(i=>i.Id == id);
            return category.Name;

        }
    }
}
