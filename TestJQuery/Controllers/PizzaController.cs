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
                .Where(op => op.Order.UserId == userId && op.Order.OrdSt == OrderStatus.InCart)
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

            // Genera URL assoluti dinamici in base alla richiesta corrente
            var successUrl = Url.Action("Success", "Pizza", null, Request.Scheme);
            var cancelUrl = Url.Action("Cart", "Pizza", null, Request.Scheme);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = total,
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
                SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Json(new { sessionId = session.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int num)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var itEsiste = await _context.OrderedPizzas
                .FirstOrDefaultAsync(piz => piz.Id == num && piz.Order.UserId == userId);

            if(itEsiste == null)
            {
                return Json(new { success = false, message = "Pizza inesistente" });
            }

            _context.OrderedPizzas
                .Remove(itEsiste);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pizza eliminata" });
        }

        [HttpGet]
        public async Task<IActionResult> Success(string session_id)
        {
            // 1. Verifica che session_id sia presente
            if (string.IsNullOrEmpty(session_id))
            {
                TempData["ErrorMessage"] = "Sessione di pagamento non valida";
                return RedirectToAction("Cart");
            }

            // 2. Verifica il pagamento con Stripe
            try
            {
                var sessionService = new SessionService();
                var stripeSession = await sessionService.GetAsync(session_id);

                // 3. Controlla che il pagamento sia completato
                if (stripeSession.PaymentStatus != "paid")
                {
                    TempData["ErrorMessage"] = "Pagamento non completato";
                    return RedirectToAction("Cart");
                }

                // 4. Trova l'ordine in carrello dell'utente
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.UserId == userId && o.OrdSt == OrderStatus.InCart);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Ordine non trovato";
                    return RedirectToAction("Cart");
                }

                // 5. Segna l'ordine come completato
                order.OrdSt = OrderStatus.Completed;
                order.OrderDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // 6. Passa i dati alla view
                ViewData["OrderId"] = order.Id;
                ViewData["TotalAmount"] = stripeSession.AmountTotal / 100m; // Converti centesimi in euro
                ViewData["PaymentIntentId"] = stripeSession.PaymentIntentId;

                return View();
            }
            catch (StripeException ex)
            {
                // Errore di comunicazione con Stripe
                TempData["ErrorMessage"] = "Errore durante la verifica del pagamento: " + ex.Message;
                return RedirectToAction("Cart");
            }
        }


    }



}
