using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinesRules.Entities;
using Entities.DTO;
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
    /// Controller class for entity methods.
    /// </summary>
    [SwaggerTag("Controller for entity methods")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly EntitiesBR entitiesBR;
        private readonly IMapper mapper;

        /// <summary>
        /// Class Constructor.
        /// </summary>
        public EntityController(EntitiesBR entitiesBR, IMapper mapper)
        {
            this.entitiesBR = entitiesBR;
            this.mapper = mapper;
        }

        /// <summary>
        /// API that allows retrieving all entity data.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <returns>List of entity data</returns>
        /// <response code="200">List of companies</response>   
        /// <response code="204">Companies not found</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Entity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {

            var entities = await this.entitiesBR.GetAllEntities(page, pageSize, columnName, orderDesc);
            if (entities.IsListObjectNull() || entities.IsEmptyListObject()) { return NoContent(); }

            return Ok(entities);

        }

        /// <summary>
        /// API that allows retrieving a pagination object with all entity data.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param> 
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <returns>Pagination object with list of entity data</returns>
        /// <response code="200">Pagination object with list of companies</response>   
        /// <response code="204">Companies not found</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="500">Internal server error</response> 
        [HttpGet("Paged")]
        [ProducesResponseType(typeof(IPagedResult<Entity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            var entities = await this.entitiesBR.GetAllEntitiesPaged(page, pageSize, columnName, orderDesc);
            if (entities.IsNull()) { return NoContent(); }
            if (entities.Results.IsListObjectNull() || entities.Results.IsEmptyListObject()) { return NoContent(); }

            return Ok(entities);
        }

        /// <summary>
        /// API that returns an entity based on its Id.
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity object</returns>
        /// <response code="200">Company object</response>   
        /// <response code="204">Company not found</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="500">Internal server error</response>   
        [HttpGet("{id:guid}", Name = "EntityById")]
        [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {

            var entity = await this.entitiesBR.GetEntityById(id);
            if (entity.IsEmptyObject() || entity.IsObjectNull()) { return NoContent(); }

            return Ok(entity);

        }

        /// <summary>
        /// API that creates a new entity.
        /// </summary>
        /// <param name="entityRegister">Entity object for registration</param>
        /// <returns>Created entity object</returns>
        /// <response code="201">Created entity object</response>
        /// <response code="400">Erroneous request, invalid object</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="500">Internal server error</response>   
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Entity), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] EntityRegisterModel entityRegister)
        {
            if (entityRegister.IsObjectNull()) { return BadRequest(new ResponseMessage { Message = "Entity object is null" }); }
            if (!ModelState.IsValid) { return BadRequest(new ResponseMessage { Message = "Invalid model object" }); }

            var entity = mapper.Map<Entity>(entityRegister);
            await this.entitiesBR.CreateEntity(entity);
            if (entity.IsEmptyObject()) { return BadRequest(new ResponseMessage { Message = "Entity Object is not Created" }); }

            return CreatedAtRoute("EntityById", new { id = entity.Id }, entity);
        }

        /// <summary>
        /// API that updates the data of an existing entity registered in the system.
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="entity">Entity object with updated data</param>
        /// <returns>Entity object with updated data</returns>
        /// <response code="200">Entity object with updated data</response>
        /// <response code="400">Erroneous request, invalid object, invalid Id</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="404">Data not found</response>   
        /// <response code="500">Internal server error</response>   
        [HttpPut("{id:guid}/Edit")]
        [ProducesResponseType(typeof(EntityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] Entity entity)
        {

            if (id.Equals(Guid.Empty)) { return BadRequest(new ResponseMessage { Message = "Id is Empty" }); }
            if (entity.IsObjectNull()) { return BadRequest(new ResponseMessage { Message = "Entity Object is Null" }); }
            if (!ModelState.IsValid) { return BadRequest(new ResponseMessage { Message = "Invalid model object" }); }

            var secuence = await this.entitiesBR.UpdateEntity(id, entity);

            if (secuence.IsObjectNull()) { return NotFound(); }

            return Ok(secuence);

        }

        /// <summary>
        /// API that allows deleting an entity record by its Id.
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>No content</returns>
        /// <response code="204">Entity object deleted, no content</response>
        /// <response code="400">Erroneous request, invalid Id</response>   
        /// <response code="401">Unauthorized</response>   
        /// <response code="403">Forbidden</response>   
        /// <response code="404">Data not found</response>   
        /// <response code="405">Not allowed to delete record</response>   
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id:guid}/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty)) { return BadRequest(new ResponseMessage { Message = "Id is Empty" }); }

            await this.entitiesBR.DeleteEntity(id);

            return NoContent();
        }
    }
}
