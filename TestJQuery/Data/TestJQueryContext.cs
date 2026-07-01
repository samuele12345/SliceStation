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

            modelBuilder.Entity<OrderedPizza>()
                .HasOne(o => o.Order)
                .WithMany(orP => orP.OrderedPizzas)
                .HasForeignKey(fk => fk.OrderId);

            modelBuilder.Entity<OrderedPizza>()
                .HasOne(p => p.Pizza)
                .WithMany(op => op.OrderedPizzas)
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(au => au.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(au => au.UserId);

            modelBuilder.Entity<Pizza>().HasData(
                    new Pizza
                    {
                        Id = 1,
                        Name = "Margherita",
                        Description = "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil.",
                        Price = 4.99m
                    },
                    new Pizza
                    {
                        Id = 2,
                        Name = "Marinara",
                        Description = "Classic pizza with tomato sauce, garlic, and oregano.",
                        Price = 4.99m
                    },
                    new Pizza
                    {
                        Id = 3,
                        Name = "Quattro Formaggi",
                        Description = "Pizza with four different types of cheese: mozzarella, gorgonzola, parmesan, and ricotta.",
                        Price = 5.99m
                    },
                    new Pizza
                    {
                        Id = 4,
                        Name = "Capricciosa",
                        Description = "Pizza with tomato sauce, mozzarella, ham, mushrooms, artichokes, and olives.",
                        Price = 6.49m   
                    },
                    new Pizza
                    {
                        Id = 5,
                        Name = "Prosciutto",
                        Description = "Pizza with tomato sauce, mozzarella, and prosciutto.",
                        Price = 5.49m
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<OrderedPizza> OrderedPizzas { get; set; }
    }


        

}
