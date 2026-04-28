using Microsoft.AspNetCore.Builder;
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
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
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
