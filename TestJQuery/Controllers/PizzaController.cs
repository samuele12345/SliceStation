using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJQuery.Data;

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
                    p.Price
                })
                .ToListAsync();

            return Json(pizzas);
        }

        
    }
}
