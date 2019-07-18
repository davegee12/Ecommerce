using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users {get;set;}

        public DbSet<Product> Products {get;set;}

        public DbSet<Order> Orders {get;set;}

    }
}