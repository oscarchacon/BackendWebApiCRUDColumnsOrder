using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Middleware;

namespace WebApi.Extensions
{
    /// <summary>
    /// Static extension class for App configuration.
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// Function that allows configuring the Swagger documentation for the App.
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{description.GroupName.ToUpperInvariant()}");
                }

                c.RoutePrefix = string.Empty;
            });

        }

        /// <summary>
        /// Function that allows configuring the middlewares to be used with the App.
        /// </summary>
        /// <param name="app"></param>
        public static void UseMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
