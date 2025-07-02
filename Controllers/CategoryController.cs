using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategeoy categeoy;
        E_CommerceContext context;

        public CategoryController(ICategeoy categeoy, E_CommerceContext context)
        {
            this.categeoy = categeoy;
            this.context = context;
        }
        public IActionResult AllCategory()
        {
            ProductRepo productRepo = new ProductRepo(context);

            CategoryRepo categoryRepo = new CategoryRepo(context);
            List<Category> categories = categoryRepo.GetAll();
         
            return View("AllCategory", categories);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult AddCategory()
        {
            return View("AddCategory");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult SaveAdd(Category category)
        {
            CategoryRepo categoryRepo = new CategoryRepo(context);
            if (ModelState.IsValid)
            {
                try
                {
                    categoryRepo.Create(category);
                    categoryRepo.Save();
                    return RedirectToAction("AllCategory");

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);


                }


            }
            return View("AddCategory", category);

        }
     

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Update(int id)
        {
            Category category1 = new Category();
            CategoryRepo categoryRepo = new CategoryRepo(context);
            Category category = categoryRepo.GetById(id);
            category1.Name = category.Name;
            return View("Update", category1);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult SaveEdit(int id, Category category)
        {
            CategoryRepo categoryRepo = new CategoryRepo(context);
            if (ModelState.IsValid)
            {
                category.Id = id;
                categoryRepo.Update(id, category);
                return RedirectToAction("AllCategory");
            }
            return View("Update", category);
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            CategoryRepo categoryRepo = new CategoryRepo(context);
            categoryRepo.Delete(id);
            categoryRepo.Save();
            return RedirectToAction("AllCategory");
        }
    }
}
