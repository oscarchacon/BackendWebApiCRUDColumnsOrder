using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// Clase Estatica para las Extensiones de Services.
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
        /// Configura el contexto de la conexion para las Base de datos, conjunto con el string de conexión.
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">Configuration</param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(option =>
                option.UseSqlite("Data Source=./storage/DBstorage.db"));
        }

        /// <summary>
        /// Register the Swagger generator, defining one or more Swagger documents
        /// </summary>
        /// <param name="services">Services</param>
        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                var contact = new OpenApiContact()
                {
                    Name = SwaggerConfiguration.ContactName,
                    Url = new Uri(SwaggerConfiguration.ContactUrl)
                };
                swagger.SwaggerDoc(SwaggerConfiguration.DocNameV1,
                        new OpenApiInfo
                        {
                            Title = SwaggerConfiguration.DocInfoTitle,
                            Version = SwaggerConfiguration.DocInfoVersion,
                            Description = SwaggerConfiguration.DocInfoDescription,
                            Contact = contact
                        });

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
                    //In = ParameterLocation.Header,
                    Type = SwaggerConfiguration.SecurityApiKeyType
                    //Type = SecuritySchemeType.ApiKey
                });

                swagger.AddSecurityRequirement(security);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "WebApiEasySwagger.xml");
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

                swagger.EnableAnnotations();
            });

        }

        /// <summary>
        /// Configura Los controladores, para que puedan ser usados por NewtonsoftJson
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
        /// Agrega la inyección de dependencias del mapeo automático de la libreria AutoMapper
        /// </summary>
        /// <param name="services">Services.</param>
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }


        /// <summary>
        /// Configura los Contenedores de los Repositorios (Repository Wrappers).
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
