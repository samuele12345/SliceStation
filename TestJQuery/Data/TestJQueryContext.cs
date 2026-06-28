using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJQuery.Models;

namespace TestJQuery.Data
{
    public class TestJQueryContext : IdentityDbContext<ApplicationUser>
    {
        public TestJQueryContext(DbContextOptions<TestJQueryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }


    }


        

}
