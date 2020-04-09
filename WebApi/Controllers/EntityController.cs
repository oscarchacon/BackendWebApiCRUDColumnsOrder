using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinesRules.Entities;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Utils;

namespace WebApi.Controllers
{
    /// <summary>
    /// Clase Controlador para métodos de Entidad
    /// </summary>
    [SwaggerTag("Controlador para métodos de Entidad")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly EntitiesBR entitiesBR;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public EntityController(EntitiesBR entitiesBR, IMapper mapper)
        {
            this.entitiesBR = entitiesBR;
            this.mapper = mapper;
        }

        /// <summary>
        /// API que permite obtener todos los datos de Entidades
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Lista de Datos de Entidad</returns>
        /// <response code="200">Lista de Empresas</response>   
        /// <response code="204">Empresas no encontrados</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="500">Error Interno del servidor</response> 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Entity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult Get(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = this.entitiesBR.GetAllEntities(page, pageSize, columnName, orderDesc);
                if (entities.IsListObjectNull() || entities.IsEmptyListObject()) { return NoContent(); }

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error {ex.Message}" });
            }
        }


        /// <summary>
        /// API que permite obtener un Objeto de Paginación con todas los datos de Entidades
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param> 
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Objeto de Paginación con Lista de Datos de Entidades</returns>
        /// <response code="200">Objeto de Paginación con Lista de Empresas</response>   
        /// <response code="204">Empresas no encontradas</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="500">Error Interno del servidor</response> 
        [HttpGet("Paged")]
        [ProducesResponseType(typeof(IPagedResult<Entity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult GetPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = this.entitiesBR.GetAllEntitiesPaged(page, pageSize, columnName, orderDesc);
                if (entities.IsNull()) { return NoContent(); }
                if (entities.Results.IsListObjectNull() || entities.Results.IsEmptyListObject()) { return NoContent(); }

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error {ex.Message}" });
            }
        }

        /// <summary>
        /// Api que retorna una Entidad a partir de su Id
        /// </summary>
        /// <param name="id">Id de Entidad</param>
        /// <returns>Objeto Entidad</returns>
        /// <response code="200">Objeto Empresa</response>   
        /// <response code="204">Empresa no encontrado</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="500">Error Interno del servidor</response>   
        [HttpGet("{id}", Name = "EntityById")]
        [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult Get(Guid id)
        {
            try
            {
                var entity = this.entitiesBR.GetEntityById(id);
                if (entity.IsEmptyObject() || entity.IsObjectNull()) { return NoContent(); }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error {ex.Message}" });
            }
        }

        /// <summary>
        /// Api que crea una nueva Entidad
        /// </summary>
        /// <param name="entityRegister">Objeto Entidad para registro</param>
        /// <returns>Objeto Enitdad Creada</returns>
        /// <response code="201">Objeto Entidad Creada</response>
        /// <response code="400">Petición Erronea, Objeto no válido</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="500">Error Interno del servidor</response>   
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Entity), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody]EntityRegisterModel entityRegister)
        {
            if (entityRegister.IsObjectNull()) { return BadRequest(new ResponseMessage { Message = "Entity object is null" }); }
            if (!ModelState.IsValid) { return BadRequest(new ResponseMessage { Message = "Invalid model object" }); }
            try
            {
                var entity = mapper.Map<Entity>(entityRegister);
                this.entitiesBR.CreateEntity(entity);
                if (entity.IsEmptyObject()) { return BadRequest(new ResponseMessage { Message = "Entity Object is not Created" }); }

                return CreatedAtRoute("EntityById", new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Api que actualiza los datos de una Entidad existente registrado en el sistema
        /// </summary>
        /// <param name="id">Id Entidad</param>
        /// <param name="entity">Objeto Entidad, con los datos actualizados</param>
        /// <returns>Objeto Entidad con datos actualizados</returns>
        /// <response code="200">Objeto Entidad con Datos Actualizados</response>
        /// <response code="400">Petición Erronea, Objeto no válido, Id no válido</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="404">Datos no encontrados</response>   
        /// <response code="500">Error Interno del servidor</response>   
        [HttpPut("{id}/Edit")]
        [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult Put(Guid id, [FromBody]Entity entity)
        {
            try
            {
                if (id.Equals(Guid.Empty)) { return BadRequest(new ResponseMessage { Message = "Id is Empty" }); }
                if (entity.IsObjectNull()) { return BadRequest(new ResponseMessage { Message = "Entity Object is Null" }); }
                if (!ModelState.IsValid) { return BadRequest(new ResponseMessage { Message = "Invalid model object" }); }

                bool secuence = this.entitiesBR.UpdateEntity(id, entity);

                if (!secuence) { return NotFound(); }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Api que permite eliminar registro de Entidad a partir de su Id
        /// </summary>
        /// <param name="id">Id Entidad</param>
        /// <returns>Sin contenido</returns>
        /// <response code="204">Objeto Entidad eliminado, sin contenido</response>
        /// <response code="400">Petición Erronea, Id no válido</response>   
        /// <response code="401">Sin Autorización</response>   
        /// <response code="403">Sin Privilegios</response>   
        /// <response code="404">Datos no encontrados</response>   
        /// <response code="405">No se permite borrar registro</response>   
        /// <response code="500">Error Interno del servidor</response>
        [HttpDelete("{id}/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty)) { return BadRequest(new ResponseMessage { Message = "Id is Empty" }); }

                var secuence = this.entitiesBR.DeleteEntity(id);
                if (secuence.IsObjectNull()) { return NotFound(); }

                if (!(bool)secuence) { return StatusCode(StatusCodes.Status405MethodNotAllowed, new ResponseMessage { Message = "Not allowed to delete Entity registry." }); }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}