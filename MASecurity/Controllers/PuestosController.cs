using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MASecurity.DataContext;
using MASecurity.Models;
using MASecurity.Services;
using System.Security.Claims;

namespace MASecurity.Controllers
{
    public class PuestosController : Controller
    {
        private readonly IPuestoService _puestoService;

        public PuestosController(IPuestoService puestoService)
        {
            _puestoService = puestoService;
        }

        // GET: Puestos
        public async Task<IActionResult> Index()
        {
            if (GetRole().Equals("Administrador"))
            {
                return View(await _puestoService.GetPuestos());
            }

            else
            {
                return Redirect("~/Home");
            }
        }

        // GET: Puestos/Create
        public IActionResult Create()
        {
            if (GetRole().Equals("Administrador"))
            {
                return View();
            }

            else
            {
                return Redirect("~/Home");
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PuestoID,Nombre,Descripcion")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                _puestoService.Create(puesto);                
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        
        // GET: Puestos/Delete/5
        public IActionResult Delete(int id)
        {
            if (GetRole().Equals("Administrador"))
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var puesto = _puestoService.Find(id);

                if (puesto == null)
                {
                    return NotFound();
                }

                return View(puesto);
            }

            else
            {
                return Redirect("~/Home");
            }
        }

        // POST: Puestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _puestoService.Delete(id);
            
            return RedirectToAction(nameof(Index));
        }

        private string GetRole()
        {
            var usuario = $"{User.Identity.Name}";
            var claims = new List<Claim>();

            foreach (var claim in User.Claims.ToList())
            {
                claims.Add(claim);
            }

            TempData["Rol"] = $"{claims[2].Value}";

            return TempData["Rol"].ToString();
        }
    }
}
