using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJQuery.Data;
using System.Security.Claims;

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
