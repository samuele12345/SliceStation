using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using TestJQuery.Data;
using TestJQuery.Models;

namespace TestJQuery.Controllers
{
    public class PizzaController : Controller
    {
        private readonly TestJQueryContext _context;
        private readonly IConfiguration _configuration;

        public PizzaController(TestJQueryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Cart()
        {
            ViewData["StripePublishableKey"] = _configuration["Stripe:PublishableKey"];
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CartData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.OrderedPizzas
                .Where(op => op.Order.UserId == userId)
                .Select(op => new
                {
                    op.Id,
                    op.Quantity,
                    op.PizzaName,
                    op.PriceSnapshot
                })
                .ToListAsync();

            return Json(cartItems);
        }

        

        [HttpPost]
        public async Task<IActionResult> PostOrd(int pizzaId, int quantity, string pizzaName, decimal pizzaPrice)
        {
            try
            {
                // ← Debug: stampa parametri ricevuti
                Console.WriteLine($"Ricevuto: pizzaId={pizzaId}, quantity={quantity}, pizzaName={pizzaName ?? "NULL"}, price ={pizzaPrice}");

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, error = "Log before ordering" });
                }

                var cartOrd = await _context.Orders
                    .Include(o => o.OrderedPizzas)
                    .FirstOrDefaultAsync(o => o.UserId == userId && o.OrdSt == OrderStatus.InCart);

                if(cartOrd == null)
                {
                    cartOrd = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        OrdSt = OrderStatus.InCart,
                        OrderedPizzas = new List<OrderedPizza>()
                    };

                    _context.Orders.Add(cartOrd);
                    await _context.SaveChangesAsync();

                    // ← RICARICA l'ordine con le pizze
                    cartOrd = await _context.Orders
                        .Include(o => o.OrderedPizzas)
                        .FirstAsync(o => o.Id == cartOrd.Id);
                }

                var existingPizza = cartOrd.OrderedPizzas
                    .FirstOrDefault(op => op.PizzaId == pizzaId);

                if(existingPizza != null)
                {
                    existingPizza.Quantity += quantity;
                    existingPizza.PriceSnapshot += pizzaPrice;
                }
                else
                {
                    _context.OrderedPizzas.Add(new OrderedPizza
                    {
                        OrderId = cartOrd.Id,
                        PizzaId = pizzaId,
                        Quantity = quantity,
                        PizzaName = pizzaName,
                        PriceSnapshot = pizzaPrice
                    });
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Pizza added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message, details = ex.InnerException?.Message });
            }
        }

        

        [HttpGet]
        public async Task<IActionResult> PizzaMethod(string? search)
        {
            var query = _context.Pizzas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            var pizzas = await query
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Image
                })
                .ToListAsync();

            return Json(pizzas);
        }


        [HttpPost]
        public async Task<IActionResult> stripeCheckout(long total)
        {
            if(total == 0)
            {
                return Json(new { success = false, message = "Inserire dei prodotti nel carrello" });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = total, // ← GIÀ IN CENTESIMI dal frontend!
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Ordine Pizzeria",
                                Description = "Ordine dal carrello"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:5218/Pizza/Success",
                CancelUrl = "https://localhost:5218/Pizza/Cart"
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Json(new { sessionId = session.Id });
        }

        
    }


}
