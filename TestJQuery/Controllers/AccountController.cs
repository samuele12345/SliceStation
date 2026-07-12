using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestJQuery.ViewModels;
using TestJQuery.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestJQuery.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verifica che email non sia vuota
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("Email", "Email is required");
                return View(model);
            }

            // Normalizza email
            var email = model.Email.Trim().ToLower();

            bool itemExists = await _userManager.Users.AnyAsync(u => u.Email == email);

            if (itemExists)
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            ApplicationUser aUser = new ApplicationUser
            {
                Email = email,
                UserName = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper(),
                EmailConfirmed = false,
                FullName = $"{model.Name?.Trim()} {model.Surname?.Trim()}",
                Address = model.Address?.Trim(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(aUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(aUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect email or password");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CurrentUserData()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return Json(new
                {
                    success = false
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    name = user.FullName,
                    email = user.Email,
                    address = user.Address
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CurrentUserData([FromBody] UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Dati non validi",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(em => em.ErrorMessage)
                });

            }

            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return Json(new { success = false, message = "Utente non trovato" });
            }

            user.Email = model.Email;
            user.FullName = model.Name;
            user.UserName = model.Email;
            user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Json(new {
                    success = true,
                    message = "Utente aggiornato con successo",
                    name = user.FullName,
                    email = user.Email,
                    address = user.Address
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Errore nel salvataggio",
                    errors = result.Errors.Select(e => e.Description)
                });
            }
        }


    }
}
