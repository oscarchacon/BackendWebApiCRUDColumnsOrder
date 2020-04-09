using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    /// <summary>
    /// Clase de Configuración de Swagger.
    /// </summary>
    public class SwaggerConfiguration
    {
        /// <summary>
        /// <para>Endpoint Description</para>
        /// </summary>
        public const string EndpointDescription = "Endpoint Description";

        /// <summary>
        /// <para>/swagger/v1/swagger.json</para>
        /// </summary>
        public const string EndpointUrl = "/swagger/v1/swagger.json";

        /// <summary>
        /// <para>Contact Name</para>
        /// </summary>
        public const string ContactName = "Contact Name";

        /// <summary>
        /// <para>Contact URL</para>
        /// </summary>
        public const string ContactUrl = "http://www.urlhere.extension";

        /// <summary>
        /// <para>Doc Name v1</para>
        /// </summary>
        public const string DocNameV1 = "v1";

        /// <summary>
        /// <para>Doc Info Title</para>
        /// </summary>
        public const string DocInfoTitle = "Doc Info Title";

        /// <summary>
        /// <para>Doc Info Version</para>
        /// </summary>
        public const string DocInfoVersion = "Doc Info Version";

        /// <summary>
        /// <para>Doc Info Description</para>
        /// </summary>
        public const string DocInfoDescription = "Doc Info Description";

        /// <summary>
        /// <para>Security Type Name</para>
        /// </summary>
        public const string SecurityTypeName = "Bearer";

        /// <summary>
        /// <para>JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"</para>
        /// </summary>
        public const string SecurityApiKeyDescription = "JWT Authorization header using the Bearer scheme. Example: \\\"Authorization: Bearer {token}\\\"";

        /// <summary>
        /// <para>Authorization</para>
        /// </summary>
        public const string SecurityApiKeyName = "Authorization";

        /// <summary>
        /// <para>header</para>
        /// </summary>
        //public const string SecurityApiKeyIn = "header";
        public const ParameterLocation SecurityApiKeyIn = ParameterLocation.Header;

        /// <summary>
        /// <para>apiKey</para>
        /// </summary>
        //public const string SecurityApiKeyType = "apiKey";
        public const SecuritySchemeType SecurityApiKeyType = SecuritySchemeType.ApiKey;
    }
}
