using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly E_CommerceContext context;
        private readonly UserManager<UserApp> userManager; 

        public PaymentController(E_CommerceContext context,UserManager<UserApp> userManager)
        {
            this.context = context;
            UserManager = userManager;
        }

        public UserManager<UserApp> UserManager { get; }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var customer = context.Customers.FirstOrDefault(c => c.UserAppId == user.Id);
            if (customer == null)
                return NotFound("Customer not found");

            var payments = await context.payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Product)
                .Where(p => p.Order.CustomerId == customer.Id)
                .ToListAsync();

            return View("Index", payments);
        }

        [HttpGet]
        public IActionResult PaymentMethod(int id)
        {
            var order = context.OrderDetailes
                .Include(o => o.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var paymentVM = new PaymentWIthOrderVM
            {
                OrderId = order.Id,
                TotalPrice = order.TotalPrice,
                Quantity = order.Quantity,
                ProductName = order.Product.Name 
            };

            return View("PaymentMethod", paymentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SavePaymentMethod(PaymentWIthOrderVM paymentVM)
        {
            if (ModelState.IsValid)
            {
                var order = context.OrderDetailes
                    .Include(o => o.Product)
                    .FirstOrDefault(o => o.Id == paymentVM.OrderId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                var payment = new Payment
                {

                    PaymentMethod = paymentVM.PaymentMethod,
                    OrderID = paymentVM.OrderId
                };

                context.payments.Add(payment);
                context.SaveChanges();

                TempData["Message"] = "Payment saved successfully!";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Invalid payment details. Please try again.";
            return View("PaymentMethod", paymentVM);
        }
    }
}
