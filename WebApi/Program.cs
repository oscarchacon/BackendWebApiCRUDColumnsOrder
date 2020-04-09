using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    /// <summary>
    /// Clase Principal, contiene el Main
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Función Main
        /// </summary>
        /// <param name="args">Argumentos del Main</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Función Creadora del Host
        /// </summary>
        /// <param name="args">Argumentos</param>
        /// <returns>Ensamblado del Host</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
