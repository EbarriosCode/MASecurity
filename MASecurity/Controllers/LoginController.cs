using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MASecurity.Models;
using MASecurity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MASecurity.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service) => _service = service;
        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] Usuario model)
        {
            var users = await _service.Get();

            if (ModelState.IsValid)
            {
                var login = _service.ValidateUser(model);

                if (login != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.UserId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, login.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Role, login.Rol.RolName));
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal/*, new AuthenticationProperties { IsPersistent = model.RememberMe }*/);

                    HttpContext.Session.SetString("Rol", login.Rol.RolName);
                   
                    return Redirect("~/Home/Index");
                }

            }

            model.UserName = string.Empty;
            ViewData["ErrorLogin"] = $"Usuario y/o Contraseña incorrectos";

            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("~/Login");
        }
    }
}