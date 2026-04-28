using AutoMapper;
using Entities.DTO;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils.Paged;
using Entities.Utils.Paged.Interfaces;
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
            this.CreateMap<EntityDTO, Entity>().ReverseMap();
            this.CreateMap<PagedResult<EntityDTO>, PagedResult<Entity>>().ReverseMap();
        }
    }
}
