using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestJQuery.Data;
using TestJQuery.Models;

namespace TestJQuery.Controllers
{
    public class PizzaController : Controller
    {
        private readonly TestJQueryContext _context;

        public PizzaController(TestJQueryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // TODO: Questo è temporaneo - dopo creerai CartItem
            var cartItems = await _context.OrderedPizzas
                .Include(op => op.Pizza)
                .Include(op => op.Order)
                .Where(op => op.Order.UserId == userId)
                .ToListAsync();

            return View(cartItems);
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

        
    }


}
