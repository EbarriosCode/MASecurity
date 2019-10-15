using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MASecurity.Services;
using Microsoft.AspNetCore.Authorization;
using MASecurity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MASecurity.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadosController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }
        
        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            GetRole();

            var empleados = await _empleadoService.GetEmpleados();
            return View(empleados);
        }

        //// GET: Empleadoes/Details/5
        public IActionResult Details(int id)
        {
            GetRole();

            if (id <= 0)
            {
                return NotFound();
            }

            var empleado = _empleadoService.Find(id);

            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            if(GetRole().Equals("Administrador"))
            {

                ViewData["PuestoID"] = new SelectList(_empleadoService.GetPuestos(), "PuestoID", "Nombre");
                return View();                
            }

            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpleadoID,Nombre,Telefono,Direccion,PuestoID")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _empleadoService.Create(empleado);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PuestoID"] = new SelectList(_empleadoService.GetPuestos(), "PuestoID", "Nombre", empleado.PuestoID);
            return View(empleado);
        }

        // GET: Empleadoes/Edit/5
        public IActionResult Edit(int id)
        {
            if (GetRole().Equals("Administrador") || GetRole().Equals("RH"))
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var empleado = _empleadoService.Find(id);
                if (empleado == null)
                {
                    return NotFound();
                }
                ViewData["PuestoID"] = new SelectList(_empleadoService.GetPuestos(), "PuestoID", "Nombre", empleado.PuestoID);
                return View(empleado);
            }

            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpleadoID,Nombre,Telefono,Direccion,PuestoID")] Empleado empleado)
        {
            if (id != empleado.EmpleadoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoService.Edit(empleado);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_empleadoService.EmpleadoExists(empleado.EmpleadoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PuestoID"] = new SelectList(_empleadoService.GetPuestos(), "PuestoID", "Nombre", empleado.PuestoID);
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public IActionResult Delete(int id)
        {
            if (GetRole().Equals("Administrador"))
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var empleado = _empleadoService.Find(id);

                if (empleado == null)
                {
                    return NotFound();
                }

                return View(empleado);
            }

            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empleadoService.Delete(id);

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
