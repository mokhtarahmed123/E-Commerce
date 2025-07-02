using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        E_CommerceContext _context;
        private readonly IProduct product;

        public ProductController(IProduct product, E_CommerceContext context)
        {
            this.product = product;
            this._context = context;
        }

        public ActionResult AllProduct()
        {
            List<Product> products = product.GetAll();
            return View("AllProduct", products);
        }
        [HttpGet]

        public ActionResult AddProduct()
        {
            //CategoryRepo categoryRepo = new CategoryRepo(_context);
            //ProductViewModelWithCategory productViewModel = new ProductViewModelWithCategory();
            ////List<Category> categories = categoryRepo.GetAll();
            //List<Category> categories = _context.Categorys.ToList();
            //productViewModel.categories = categories;
            ////SelectList selectListItems = categories;
            //ViewBag.Categories = new SelectList(categories, "Id", "Name");
            //return View("AddProduct", productViewModel);
            // Get categories from database
            List<Category> categories = _context.Categorys.ToList();

            // Fill the view model
            ProductViewModelWithCategory productViewModel = new ProductViewModelWithCategory
            {
                categories = categories
            };

            // Return the view with the model
            return View("AddProduct", productViewModel);
        }

        [HttpPost]
        public ActionResult SaveAdd(ProductViewModelWithCategory pro)
        {
            //ProductRepo productRepo = new ProductRepo(_context);
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        Product product = new Product();
            //        product.Name = pro.Name;
            //        product.Price = pro.Price;
            //        product.Quantity = pro.Quantity;
            //        product.UrlImage = pro.UrlImage;
            //        product.CategoryId = pro.CId;
            //        productRepo.Create(product);
            //        productRepo.Save();
            //        return RedirectToAction("AllProduct");
            //    }
            //    catch (Exception ex)
            //    {

            //        ModelState.AddModelError("", ex.Message);
            //    }

            //}
            //ViewBag.Categories = new SelectList(_context.Categorys, "Id", "Name");

            //return View("AddProduct", pro);
            ProductRepo productRepo = new ProductRepo(_context);

            // 👇 دي بتمنع الخطأ الزايد
            ModelState.Remove("categories");

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product
                    {
                        Name = pro.Name,
                        Price = pro.Price,
                        Quantity = pro.Quantity,
                        UrlImage = pro.UrlImage,
                        CategoryId = pro.CId
                    };

                    productRepo.Create(product);
                    productRepo.Save();
                    return RedirectToAction("AllProduct");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Populate the categories again before returning the view
            pro.categories = _context.Categorys.ToList();
            return View("AddProduct", pro);

        }

        [HttpGet]
        public ActionResult Update(int id) {
            #region
            //  ProductViewModelWithCategory product = new ProductViewModelWithCategory();
            //  ProductRepo productRepo = new ProductRepo(_context);
            //Product product1 =   productRepo.GetById(id);
            //  product.Id = id;
            //  product.Name = product1.Name;
            //  product.Price = (decimal)product1.Price;
            //  product.Quantity = (int)product1.Quantity;
            //  product.CId = (int)product1.CategoryId;
            //  product.UrlImage = product1.UrlImage;
            //  List <Category> cat = _context.Categorys.ToList();
            //  product.categories = cat;
            //  product.categories = _context.Categorys.ToList();

            //  //SelectList selectListItems = new SelectList(product.categories, "Id", "Name");

            //  return View("Update",product);
            #endregion
            Product product1 = _context.Product.FirstOrDefault(p => p.Id == id);

            ProductViewModelWithCategory product = new ProductViewModelWithCategory
            {
                Id = product1.Id,
                Name = product1.Name,
                Price = (decimal)product1.Price,
                Quantity = (int)product1.Quantity,
                UrlImage = product1.UrlImage,
                CId = product1.CategoryId,
                categories = _context.Categorys.ToList()
            };

            return View("Update", product);
        }
        [HttpPost]
        public IActionResult SaveEdit(int id, ProductViewModelWithCategory product) {
            if (product.Name !=null) {
                try
                {
                    Product product1 = _context.Product.FirstOrDefault(x => x.Id == id);
                    if (product1 != null)
                    {

                        product1.Name = product.Name;
                        product1.Price = (decimal)product.Price;
                        product1.Quantity = (int)product.Quantity;
                        product1.UrlImage = product.UrlImage;
                        product1.CategoryId = (int)product.CId;
                        _context.SaveChanges();
                        return RedirectToAction("AllProduct");
                    }
                }
                catch (Exception ex) {
                        ModelState.AddModelError("",ex.Message);
                }
                
            
            
            
            }
            product.categories = _context.Categorys.ToList();
            return View("Update", product);

        }

        [HttpGet]
        public void Delete(int id)
        {
            var product = _context.Product.Find(id);

            bool isUsed = _context.OrderDetailes.Any(o => o.ProductID == id);
            if (isUsed)
            {
                throw new Exception("Cannot delete product. It is used in one or more orders.");
            }

            _context.Product.Remove(product);
            _context.SaveChanges();
        }


    }
}
