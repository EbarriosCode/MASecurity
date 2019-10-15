using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MASecurity.DataContext;
using MASecurity.DataContext.Data;
using MASecurity.Middleware;
using MASecurity.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MASecurity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<SecurityContext>(options =>                
            //        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
            //        .Replace("{{DB_ENDPOINT}}", Configuration.GetValue<string>("DB_ENDPOINT"))));

            //var contexto = services.BuildServiceProvider().GetRequiredService<SecurityContext>();
            //if (Configuration.GetValue<bool>("DB_MIGRATE") == true)
            //{
            //    contexto.Database.Migrate();
            //}

            //if (Configuration.GetValue<bool>("DB_INITIALIZE") == true)
            //{
            //    DbInit.Initialize(contexto);
            //}
            
            //Configuración de Sql Server en memoria
            services.AddDbContext<SecurityContext>(options => options.UseInMemoryDatabase(databaseName: "Security_DB"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAntiforgery(o => o.HeaderName = "HSRF-TOKEN");
            
            // Middleware Inyección de Dependencias
            IoC.AddDependency(services);

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(
                options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(10);
                });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => { options.LoginPath = "/Login"; });

            services.AddAuthorization();

            // Agregar Antifogery-Token (XSRF / CSRF)
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            //services.AddDefaultIdentity<ApplicationUser>()
            //        .AddRoles<IdentityRole>()
            //        .AddDefaultUI(UIFramework.Bootstrap4)
            //        .AddEntityFrameworkStores<SecurityContext>();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            //Agregando Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Administrador");
            if (!roleCheck)
            {
                //Crear los roles y agregarlos a la bd
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            // Asignar rol de administrador al usuario principal aquí le hemos dado a nuestro recién registrado
            // ID de inicio de sesión para la administración del administrador
            ApplicationUser user = await UserManager.FindByEmailAsync("guayoswing@gmail.com");
            await UserManager.AddToRoleAsync(user, "Administrador");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });

            //CreateUserRoles(serviceProvider).Wait();
        }
    }
}
