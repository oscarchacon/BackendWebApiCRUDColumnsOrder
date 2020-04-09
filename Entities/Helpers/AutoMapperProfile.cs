using AutoMapper;
using Entities.Helpers.Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Helpers
{
    /// <summary>
    /// Clase de Configuración para hacer un auto-mapeo ocupando la libreria AutoMapper
    /// </summary>
    public class AutoMapperProfile : Profile 
    {
        /// <summary>
        /// Constructor, con los parametros necesarios para realizar el mapeo automatico
        /// </summary>
        public AutoMapperProfile()
        {
            this.CreateMap<EntityRegisterModel, Entity>();
        }
    }
}
