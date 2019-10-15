using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MASecurity.DataContext;
using MASecurity.DataContext.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASecurity
{
    public class Program
    {
        public static void Main(string[] args)
        {          
            // Obtener el IWebHost que alojará la aplicación.
            var host = CreateWebHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                // Crear la instancia del Contexto en la capa de servicios
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SecurityContext>();
              
                DBInitializer.Initialize(services);
            }
            
            host.Run();


            // Configuración con dos contenedores separados
            //var host = CreateWebHostBuilder(args).Build();

            //// Encontrar la capa de servicio
            //using (var scope = host.Services.CreateScope())
            //{
            //    // Crear la instancia del Contexto en la capa de servicios
            //    var services = scope.ServiceProvider;

            //    try
            //    {
            //        var context = services.GetRequiredService<SecurityContext>();
            //        DbInit.Initialize(context);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "Ha Ocurrido un error al llenar la DB");
            //    }
            //}

            //host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
