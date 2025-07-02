using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly E_CommerceContext context;
        private readonly UserManager<UserApp> userManager;
        private readonly SignInManager<UserApp> signInManager;
        public AccountController
                (UserManager<UserApp> _userManager,
                SignInManager<UserApp> _signInManager, E_CommerceContext _context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            context = _context;
        }
        public ActionResult Signup()
        {
            return View("SignUp");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SignupSave(SignUpViewModel newUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserApp userModel = new UserApp
        //        {
        //            UserName = newUser.UserName,
        //            PhoneNumber = newUser.PhoneNumber
        //        };

        //        IdentityResult result = await userManager.CreateAsync(userModel, newUser.Password);

        //        if (result.Succeeded)
        //        {
        //            try
        //            {
        //                await signInManager.SignInAsync(userModel, false);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("", ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }

        //    return View("SignUp", newUser);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignupSave(SignUpViewModel newUser)
        {
            CustomerRepo customerRepo = new CustomerRepo(context);  
            if (ModelState.IsValid)
            {
                UserApp userModel = new UserApp
                {
                    UserName = newUser.UserName,
                    PhoneNumber = newUser.PhoneNumber
                };

                IdentityResult result = await userManager.CreateAsync(userModel, newUser.Password);

                if (result.Succeeded)
                {
                    try
                    {
                        await signInManager.SignInAsync(userModel, false);

                        Customer customer = new Customer
                        {
                            Name = userModel.UserName,
                            PhoneNumber = userModel.PhoneNumber,
                            UserAppId = userModel.Id,
                     

                        };
                        await userManager.AddToRoleAsync(userModel, "Admin");

                        customerRepo.Create(customer);
                        await context.SaveChangesAsync();

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("SignUp", newUser);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("LogIn");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginCheck(LoginViewModel userVM)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var userModel = await userManager.FindByNameAsync(userVM.UserName);
                    if (userModel != null)
                    {
                        bool found = await userManager.CheckPasswordAsync(userModel, userVM.Password);
                        if (found)
                        {
                            await signInManager.SignInAsync(userModel, userVM.RememberMe);

                            var existingCustomer = context.Customers.FirstOrDefault(c => c.UserAppId == userModel.Id);
                            if (existingCustomer == null)
                            {
                                Customer customer = new Customer
                                {
                                    Name = userModel.UserName,
                                    PhoneNumber = userModel.PhoneNumber,
                                    UserAppId = userModel.Id
                                };
                                context.Customers.Add(customer);
                                await context.SaveChangesAsync();
                            }

                            return RedirectToAction("Index", "Home");
                        }
                    }

                    ModelState.AddModelError("", "Invalid username or password");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }

            return View("Login", userVM);
        }


        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize]

        public async Task<IActionResult> MyProfile()
        {
            UserApp user = await userManager.GetUserAsync(User);


            if (user == null)
            {
                return NotFound("User not found");
            }

            return View("MyProfile", user);
        }

    }
}
