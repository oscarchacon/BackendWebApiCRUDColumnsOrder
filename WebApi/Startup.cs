using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Extensions;

namespace WebApi
{
    /// <summary>
    /// Clase de Configuración de la API
    /// </summary>
    public class Startup
    {
        #pragma warning disable CS1591
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        #pragma warning restore CS1591


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.ConfigureIISIntegration();

            services.ConfigureDbContext(Configuration);

            services.ConfigureRepositoriesWrappers();

            services.ConfigureBusinessRules();

            services.ConfigureControllersWithJson();

            // Se instancia el mapeo automatico para los services
            services.AddMapper();

            services.ConfigureSwaggerGen();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App Object Builder</param>
        /// <param name="env">Environment Object</param>
        /// <param name="repositoryContext">Repository Context Object</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RepositoryContext repositoryContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // TODO: Utilizar si se necesita rehacer la Base de datos.
            repositoryContext.Database.EnsureDeleted();
            repositoryContext.Database.EnsureCreated();

        }
    }
}
