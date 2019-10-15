using MASecurity.DataContext;
using MASecurity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Services
{
    public interface IPuestoService
    {
        Task<List<Puesto>> GetPuestos();
        Task<bool> Create(Puesto puesto);
        Puesto Find(int PuestoID);
        Task<bool> Delete(int PuestoID);
        bool PuestoExists(int id);
    }

    public class PuestoService : IPuestoService
    {
        private readonly SecurityContext _context;

        public PuestoService(SecurityContext context) => _context = context;

        public async Task<List<Puesto>> GetPuestos()
        {
            var puestos = new List<Puesto>();

            try
            {
                puestos = await _context.Puestos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return puestos;
        }

        public async Task<bool> Create(Puesto puesto)
        {
            bool created = false;

            try
            {
                if (puesto != null)
                {
                    _context.Puestos.Add(puesto);
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

        public Puesto Find(int PuestoID)
        {
            var puesto = new Puesto();

            try
            {
                if (PuestoID > 0)
                {
                    puesto = _context.Puestos.FirstOrDefault(e => e.PuestoID == PuestoID);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return puesto;
        }

        public async Task<bool> Delete(int PuestoID)
        {
            bool deleted = false;

            try
            {
                if (PuestoID > 0)
                {
                    var empleado = Find(PuestoID);
                    _context.Puestos.Remove(empleado);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return deleted;
        }

        public bool PuestoExists(int id)
        {
            return _context.Puestos.Any(p => p.PuestoID == id);
        }
    }
}
