using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Helpers;
using AutoMapper;
using Repository.Wrappers.Interfaces;
using Repository.Wrappers;
using Entities.Models;
using BusinesRules.Entities;

namespace WebApi.Extensions
{
    /// <summary>
    /// Static class for Service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configura los Cors de Services.
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Configura la Integración de IIS.
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.Configure<IISOptions>(option =>
            {

            });
        }

        /// <summary>
        /// Configures the database context with the connection string.
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">Configuration</param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(option =>
                option.UseInMemoryDatabase("BackendWebApiCRUDColumnsOrderDb"));
        }

        /// <summary>
        /// Register the Swagger generator, defining one or more Swagger documents
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(swagger =>
            {
                var security = new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer" //The name of the previously defined security scheme.
                                ,Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                };

                swagger.AddSecurityDefinition(SwaggerConfiguration.SecurityTypeName, new OpenApiSecurityScheme
                {
                    Description = SwaggerConfiguration.SecurityApiKeyDescription,
                    Name = SwaggerConfiguration.SecurityApiKeyName,
                    In = SwaggerConfiguration.SecurityApiKeyIn,
                    Type = SwaggerConfiguration.SecurityApiKeyType
                });

                swagger.AddSecurityRequirement(security);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

                swagger.EnableAnnotations();
            });
        }

        /// <summary>
        /// Configures controllers to use NewtonsoftJson serialization.
        /// </summary>
        /// <param name="services">Services.</param>
        public static void ConfigureControllersWithJson(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(x =>
                    {
                        x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
        }

        /// <summary>
        /// Configures API versioning and the version explorer.
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        /// <summary>
        /// Agrega la inyección de dependencias del mapeo automático de la libreria AutoMapper
        /// </summary>
        /// <param name="services">Services.</param>
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
        }


        /// <summary>
        /// Configures repository wrappers for dependency injection.
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureRepositoriesWrappers(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        /// <summary>
        /// Configura todas las Reglas de Negocios
        /// </summary>
        /// <param name="services">Services.</param>
        public static void ConfigureBusinessRules(this IServiceCollection services)
        {
            services.AddScoped<EntitiesBR>();
        }
    }
}
