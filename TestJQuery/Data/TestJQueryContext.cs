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

            modelBuilder.Entity<Review>()
                .HasOne(u => u.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(ui => ui.UserId);


            modelBuilder.Entity<Pizza>().HasData(
                    new Pizza
                    {
                        Id = 1,
                        Name = "Margherita",
                        Description = "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil.",
                        Price = 4.99m,
                        Image = "/src/Margherita.jpg"
                    },
                    new Pizza
                    {
                        Id = 2,
                        Name = "Vegana",
                        Description = "Classic pizza with tomato sauce, mozzarella and vegetables",
                        Price = 4.99m,
                        Image = "/src/vegana.jpg"
                    },
                    new Pizza
                    {
                        Id = 3,
                        Name = "Quattro Formaggi",
                        Description = "Pizza with four different types of cheese: mozzarella, gorgonzola, parmesan, and ricotta.",
                        Price = 5.99m,
                        Image = "/src/quattroForm.jpg"
                    },
                    new Pizza
                    {
                        Id = 4,
                        Name = "Capricciosa",
                        Description = "Pizza with tomato sauce, mozzarella, ham, mushrooms, artichokes, and olives.",
                        Price = 6.49m,
                        Image = "/src/capricciosa.jpg"
                    },
                    new Pizza
                    {
                        Id = 5,
                        Name = "Salame",
                        Description = "Pizza with tomato sauce, mozzarella, pepperoni.",
                        Price = 5.49m,
                        Image = "/src/pepperoni.jpg"
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<OrderedPizza> OrderedPizzas { get; set; }
    }


        

}
