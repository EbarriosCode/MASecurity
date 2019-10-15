using MASecurity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MASecurity.DataContext;
using Microsoft.EntityFrameworkCore;

namespace MASecurity.Services
{
    public interface ILoginService
    {
        Usuario ValidateUser(Usuario model);
        Task<List<Usuario>> Get();
    }
    public class LoginService : ILoginService
    {
        private readonly SecurityContext _context;

        public LoginService(SecurityContext context) => _context = context;

        public Usuario ValidateUser(Usuario model)
        {
            var isValid = new Usuario();

            try
            {
                if (model != null)
                {
                    isValid = _context.Usuarios
                                      .Include(u => u.Rol)
                                      .FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isValid;
        }

        public async Task<List<Usuario>> Get()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}
