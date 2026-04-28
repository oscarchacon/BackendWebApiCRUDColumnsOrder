using AutoMapper;
using Entities.DTO;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Helpers
{
    /// <summary>
    /// Configuration class for performing automatic mapping using the AutoMapper library.
    /// </summary>
    public class AutoMapperProfile : Profile 
    {
        /// <summary>
        /// Constructor with necessary parameters to perform automatic mapping.
        /// </summary>
        public AutoMapperProfile()
        {
            this.CreateMap<EntityRegisterModel, Entity>();
            this.CreateMap<EntityDTO, Entity>().ReverseMap();
            this.CreateMap<IPagedResult<EntityDTO>, IPagedResult<Entity>>().ReverseMap();
        }
    }
}
