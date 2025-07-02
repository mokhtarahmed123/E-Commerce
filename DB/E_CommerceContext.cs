using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DB
{
    public class E_CommerceContext : IdentityDbContext<UserApp>
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderDetailes> OrderDetailes { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<UserApp> userApps { get; set; }
        public DbSet<RoleApp> RoleApps { get; set; }

        

        public E_CommerceContext() : base()
        {

        }
        public E_CommerceContext(DbContextOptions<E_CommerceContext> options) : base(options)
        {
        }

    }
}
