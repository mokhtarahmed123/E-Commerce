using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
})
.AddEntityFrameworkStores<E_CommerceContext>();

builder.Services.AddDbContext<E_CommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("E_Commerce"))
);

builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ICategeoy, CategoryRepo>();
builder.Services.AddScoped<IorderDetails, OrederDetailsRepo>();
builder.Services.AddScoped<ICustomer, CustomerRepo>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<UserApp>>();

    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    string adminUserName = "Mokhtar@";
    string adminPassword = "123456789m";
    string adminPhone = "01140778192";

    var adminUser = await userManager.FindByNameAsync(adminUserName);
    if (adminUser == null)
    {
        var user = new UserApp
        {
            UserName = adminUserName,
            PhoneNumber = adminPhone
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
