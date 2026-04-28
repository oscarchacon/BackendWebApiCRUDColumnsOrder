using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Helpers
{
    /// <summary>
    /// Configures Swagger documents for all discovered API versions.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider"/> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }


        /// <summary>
        /// Configures Swagger options.
        /// </summary>
        /// <param name="options">The SwaggerGenOptions to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in this.provider.ApiVersionDescriptions)
            {
                options.SwaggerGeneratorOptions.SwaggerDocs[description.GroupName] = CreateInfoForApiVersion(description);
            }
        }


        /// <summary>
        /// Creates an OpenApiInfo object for a given API version description.
        /// </summary>
        /// <param name="description">The API version description.</param>
        /// <returns>The OpenApiInfo object.</returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = SwaggerConfiguration.DocInfoTitle,
                Version = description.GroupName,
                Description = SwaggerConfiguration.DocInfoDescription,
                Contact = new OpenApiContact
                {
                    Name = SwaggerConfiguration.ContactName,
                    Url = new Uri(SwaggerConfiguration.ContactUrl)
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
