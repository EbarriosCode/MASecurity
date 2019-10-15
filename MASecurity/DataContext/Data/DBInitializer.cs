using MASecurity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.DataContext.Data
{
    public class DBInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var _context = new SecurityContext(serviceProvider.GetRequiredService<DbContextOptions<SecurityContext>>()))
            {
                // Agregando Roles a la BD
                if (_context.Roles.Any())
                {
                    return;
                }

                _context.Roles.AddRange(
                    new Rol { RolName = "Administrador", Permisos = "Leer,Crear,Actualizar,Borrar" },
                    new Rol { RolName = "RH", Permisos = "Leer,Actualizar" },
                    new Rol { RolName = "Operador", Permisos = "Leer" }
                 );

                _context.SaveChanges();

                // Agregando Usuarios a la BD
                if (_context.Usuarios.Any())
                {
                    return;
                }

                _context.Usuarios.AddRange(
                    new Usuario
                    {
                        UserId = new Guid(),
                        UserName = "Ebarrios",
                        Password = "Ebarrios123",
                        Name = "Eduardo Barrios",
                        RolId = _context.Roles.FirstOrDefault(r => r.RolName == "Administrador").RolId
                    },
                    new Usuario
                    {
                        UserId = new Guid(),
                        UserName = "Dralon",
                        Password = "Dralon123",
                        Name = "Denis Rosales",
                        RolId = _context.Roles.FirstOrDefault(r => r.RolName == "RH").RolId
                    },
                    new Usuario
                    {
                        UserId = new Guid(),
                        UserName = "Acabrera",
                        Password = "Acabrera123",
                        Name = "Abel Cabrera",
                        RolId = _context.Roles.FirstOrDefault(r => r.RolName == "Operador").RolId
                    },
                    new Usuario
                    {
                        UserId = new Guid(),
                        UserName = "Wrios",
                        Password = "Wrios123",
                        Name = "Walter Ríos",
                        RolId = _context.Roles.FirstOrDefault(r => r.RolName == "RH").RolId
                    },
                    new Usuario
                    {
                        UserId = new Guid(),
                        UserName = "Ctuch",
                        Password = "Ctuch123",
                        Name = "César Tuch",
                        RolId = _context.Roles.FirstOrDefault(r => r.RolName == "Operador").RolId
                    }
                );

                _context.SaveChanges();

                // Agregando Puestos a la BD
                if (_context.Puestos.Any())
                {
                    return;
                }

                _context.Puestos.AddRange(
                    new Puesto
                    {
                        Nombre = "Vendedor",
                        Descripcion = "Atender a los clientes y registrar las ventas en el sistema",
                    },

                    new Puesto
                    {
                        Nombre = "Cajero",
                        Descripcion = "Realizar los cobros a los clientes posterior a que el vendedor haya registrado la venta, y despachar productos vendidos",
                    }
                );

                _context.SaveChanges();

                // Agregando Empleados a la BD
                if (_context.Empleados.Any())
                {
                    return;
                }

                _context.Empleados.AddRange(
                   new Empleado
                   {
                       Nombre = "Elmer del Cid",
                       Telefono = 65783428,
                       Direccion = "4ta Calle 5-76 Zona 1, Retalhuleu",
                       PuestoID = _context.Puestos.FirstOrDefault(p => p.Nombre == "Vendedor").PuestoID
                   },
                   new Empleado
                   {
                       Nombre = "Jonatan Mazariegoz",
                       Telefono = 56443350,
                       Direccion = "2da Calle 5-43 Zona 5 Lot. La trinidad, Retalhuleu",
                       PuestoID = _context.Puestos.FirstOrDefault(p => p.Nombre == "Cajero").PuestoID
                   }
                   );

                _context.SaveChanges();
            }
        }
    }
}
