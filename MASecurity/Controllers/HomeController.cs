using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MASecurity.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MASecurity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {
            var usuario = $"{User.Identity.Name}";
            var claims = new List<Claim>();

            foreach (var claim in User.Claims.ToList())
            {
                claims.Add(claim);
            }

            TempData["Rol"] = $"{claims[2].Value}";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
