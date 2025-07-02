using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly E_CommerceContext context;
        private readonly UserManager<UserApp> userManager;
        private readonly SignInManager<UserApp> signInManager;
        public OrderDetailController
                (UserManager<UserApp> _userManager,
                SignInManager<UserApp> _signInManager, E_CommerceContext _context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            context = _context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = context.Customers.FirstOrDefault(c => c.UserAppId == user.Id);
            if (customer == null)
            {
                return View("AllOrders", new List<OrderWithCustomerAndProduct>());
            }

            var orderDetails = context.OrderDetailes
                .Where(od => od.CustomerId == customer.Id)
                .Include(od => od.Product)
                .ToList();

            var orderWithCustomerAndProductList = new List<OrderWithCustomerAndProduct>();

            foreach (var od in orderDetails)
            {
                orderWithCustomerAndProductList.Add(new OrderWithCustomerAndProduct
                {
                    OredrId = od.Id,
                    CustomerName = customer.Name,
                    CustomerPhoneNumber = customer.PhoneNumber,
                    ProductId = (int)od.ProductID,
                    ProductName = od.Product?.Name ?? "Unknown Product",
                    Quantity = od.Quantity,
                    TotalPrice = (int)(od.Quantity * (od.Product?.Price ?? 0))
                });
            }

            return View("AllOrders", orderWithCustomerAndProductList);
        }


        [HttpGet]
        public IActionResult CreateOrder(int id)
        {
            ProductRepo productRepo = new ProductRepo(context);
            Product product = productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            OrderWithCustomerAndProduct order = new OrderWithCustomerAndProduct
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = 1,
                TotalPrice = (decimal)product.Price,
                UrlImage = product.UrlImage
            };

            return View("CreateOrder", order);
        }
        //[HttpPost]
        //public async IActionResult SaveCreate(OrderWithCustomerAndProduct oderWithCustomerAndProduct) {
        //    if (ModelState.IsValid)
        //    {
        //        // Save the order to the database
        //        OrderDetailes orderDetailes = new OrderDetailes
        //        {
        //           Customer cust = await  
        //            ProductID = oderWithCustomerAndProduct.ProductId,
        //            Quantity = oderWithCustomerAndProduct.Quantity,
        //            TotalPrice = oderWithCustomerAndProduct.TotalPrice,
        //            C = oderWithCustomerAndProduct.CustomerName,
        //            CustomerPhoneNumber = oderWithCustomerAndProduct.CustomerPhoneNumber
        //        };
        //        e_CommerceContext.OrderDetailes.Add(orderDetailes);
        //        e_CommerceContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View("CreateOrder", oderWithCustomerAndProduct);
        //}
        [HttpPost]

        public async Task<IActionResult> SaveCreate(OrderWithCustomerAndProduct orderWithCustomerAndProduct)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var customer = context.Customers.FirstOrDefault(c => c.UserAppId == user.Id);

                if (customer == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var product = await context.Product.FindAsync(orderWithCustomerAndProduct.ProductId);

                if (product == null)
                {
                    ModelState.AddModelError("", "Product not found.");
                    return View("CreateOrder", orderWithCustomerAndProduct);
                }

                var totalPrice = orderWithCustomerAndProduct.Quantity * product.Price;

                OrderDetailes orderDetailes = new OrderDetailes
                {
                    ProductID = product.Id,
                    Quantity = orderWithCustomerAndProduct.Quantity,
                    TotalPrice = (decimal)totalPrice,
                    CustomerId = customer.Id
                };

                context.OrderDetailes.Add(orderDetailes);
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("CreateOrder", orderWithCustomerAndProduct);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            OrederDetailsRepo orederDetailsRepo = new OrederDetailsRepo(context);
            OrderDetailes orderDetail = orederDetailsRepo.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            orederDetailsRepo.Delete(id);

            return View("AllOrders");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var user = await userManager.GetUserAsync(User);


            OrederDetailsRepo orederDetailsRepo = new OrederDetailsRepo(context);
            OrderDetailes orderDetail = orederDetailsRepo.GetById(id);
            var customer = context.Customers.FirstOrDefault(c => c.UserAppId == user.Id);
            if (customer == null)
            {
                return View("AllOrders", new List<OrderWithCustomerAndProduct>());
            }
            var orderDetails = context.OrderDetailes
         .Where(od => od.CustomerId == customer.Id)
       .Include(od => od.Product)
                  .ToList();
            if (orderDetail == null)
            {
                return NotFound();
            }
            OrderWithCustomerAndProduct orderWithCustomerAndProduct = new OrderWithCustomerAndProduct
            {
                OredrId = orderDetail.Id,
            
                CustomerPhoneNumber = customer.PhoneNumber,
                ProductId = (int)orderDetail.ProductID,
                ProductName = orderDetail.Product.Name,
                Quantity = orderDetail.Quantity,
                TotalPrice = orderDetail.TotalPrice
            };
            return View("UpdateOrder", orderWithCustomerAndProduct);

        }
        [HttpPost]
        public IActionResult SaveUpdatedOrder(OrderWithCustomerAndProduct model)
        {
            if (ModelState.IsValid)
            {
                var order = context.OrderDetailes
                    .Include(o => o.Product)
                    .FirstOrDefault(o => o.Id == model.OredrId);
                if (order == null)
                {
                    ModelState.AddModelError("", "Order not found.");
                    return View("UpdateOrder", model);
                }
               
                try
                {
                    order.Quantity = model.Quantity;
                    order.TotalPrice = (decimal)(model.Quantity * order.Product.Price); 
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return RedirectToAction("Index");
            }

            return View("UpdateOrder", model);
        }

    }
}