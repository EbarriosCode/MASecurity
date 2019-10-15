using MASecurity.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MASecurity.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            // Inyectar el servicio de Authorización => Login
            services.AddTransient<ILoginService, LoginService>();

            // Inyectar el servicio de Authorización => Empleados
            services.AddTransient<IEmpleadoService, EmpleadoService>();

            // Inyectar el servicio de Authorización => Puestos
            services.AddTransient<IPuestoService, PuestoService>();

            return services;
        }
    }
}
