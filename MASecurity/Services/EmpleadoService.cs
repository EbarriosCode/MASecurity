using MASecurity.DataContext;
using MASecurity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Services
{
    public interface IEmpleadoService
    {
        Task<List<Empleado>> GetEmpleados();
        Task<bool> Create(Empleado empleado);
        Empleado Find(int EmpleadoID);
        Task<bool> Edit(Empleado empleado);
        Task<bool> Delete(int EmpleadoID);
        IEnumerable<Puesto> GetPuestos();
        bool EmpleadoExists(int id);        

    }
    public class EmpleadoService : IEmpleadoService
    {
        private readonly SecurityContext _context;

        public EmpleadoService(SecurityContext context) => _context = context;
        
        public async Task<List<Empleado>> GetEmpleados()
        {
            var empleados = new List<Empleado>();

            try
            {
                empleados = await _context.Empleados.Include(e => e.Puesto).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return empleados;
        }

        public async Task<bool> Create(Empleado empleado)
        {
            bool created = false;

            try
            {
                if(empleado != null)
                {
                    _context.Empleados.Add(empleado);
                    await _context.SaveChangesAsync();

                    created = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return created;
        }

        public Empleado Find(int EmpleadoID)
        {
            var empleado = new Empleado();

            try
            {
                if(EmpleadoID > 0)
                {
                    empleado = _context.Empleados.Include(e => e.Puesto).FirstOrDefault(e => e.EmpleadoID == EmpleadoID);                    
                }
            }
            catch (Exception)
            {
                throw;
            }

            return empleado;
        }

        public async Task<bool> Edit(Empleado empleado)
        {
            bool updated = false;
            try
            {
                if (empleado != null)
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return updated;
        }

        public async Task<bool> Delete(int EmpleadoID)
        {
            bool deleted = false;

            try
            {
                if(EmpleadoID > 0)
                {
                    var empleado = Find(EmpleadoID);
                    _context.Empleados.Remove(empleado);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return deleted;
        }

        public IEnumerable<Puesto> GetPuestos()
        {
            var puestos = new List<Puesto>();

            try
            {
                puestos = _context.Puestos.ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return puestos;
        }

        public bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.EmpleadoID == id);
        }
}
}
