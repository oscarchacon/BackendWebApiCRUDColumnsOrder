using AutoMapper;
using Entities.Helpers.Entities;
using BusinesRules.Exceptions;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged;
using Entities.Utils.Paged.Interfaces;
using Repository.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.DTO;

namespace BusinesRules.Entities;

/// <summary>
/// Business Rules class for Entities.
/// </summary>
public class EntitiesBR
{
    private readonly IRepositoryWrapper repository;
    private readonly IMapper mapper;

    public EntitiesBR(IRepositoryWrapper repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }


    /// <summary>
    /// Retrieves a list of all entity data.
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Elements per page</param>
    /// <param name="columnName">Column name to sort by</param>
    /// <param name="orderDesc">Descending sort boolean</param>
    /// <returns>List of entity data</returns>
    public async Task<IEnumerable<EntityDTO>> GetAllEntities(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
    {

        var entities = await this.repository.Entity.GetAllAsync(page, pageSize, columnName, orderDesc);
        return this.mapper.Map<IEnumerable<EntityDTO>>(entities);

    }

    /// <summary>
    /// Retrieves a pagination object with all entity data.
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Elements per page</param>
    /// <param name="columnName">Column name to sort by</param>
    /// <param name="orderDesc">Descending sort boolean</param>
    /// <returns>Pagination object with entity data list</returns>
    public async Task<IPagedResult<EntityDTO>> GetAllEntitiesPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
    {

        var entities = await this.repository.Entity.GetAllPagedAsync(page, pageSize, columnName, orderDesc);
        return this.mapper.Map<PagedResult<EntityDTO>>(entities);

    }

    /// <summary>
    /// Finds an entity by its ID asynchronously.
    /// </summary>
    /// <param name="companyId">Entity ID</param>
    /// <returns>Entity object</returns>
    public async Task<EntityDTO> GetEntityById(Guid entityId)
    {

        var company = await this.repository.Entity.GetByIdAsync(entityId);

        return this.mapper.Map<EntityDTO>(company);

    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">Entity object to register</param>
    /// <returns>Created entity object</returns>
    public async Task CreateEntity(Entity entity)
    {

        entity.RegisterDate = DateTime.UtcNow;
        await this.repository.Entity.CreateEntityAsync(entity);
        await this.repository.SaveAsync();

    }

    /// <summary>
    /// Updates data for an existing entity.
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityUpdated">Entity object with new data</param>
    /// <returns>Boolean if the action was performed</returns>
    public async Task<EntityDTO> UpdateEntity(Guid entityId, Entity entityUpdated)
    {

        var dbEntity = await this.repository.Entity.GetByIdAsync(entityId);
        if (dbEntity.IsEmptyObject()) { throw new NotFoundException("Entity not found"); }

        this.repository.Entity.UpdateEntity(dbEntity, entityUpdated);
        await this.repository.SaveAsync();

        entityUpdated.Id = entityId;

        return this.mapper.Map<EntityDTO>(entityUpdated);

    }

    /// <summary>
    /// Deletes an entity based on its ID.
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <returns>Boolean object if the action was performed</returns>
    public async Task DeleteEntity(Guid entityId)
    {

        var dbEntity = this.repository.Entity.GetById(entityId);
        if (dbEntity.IsEmptyObject()) { throw new NotFoundException("Entity not found"); }

        this.repository.Entity.DeleteEntity(dbEntity);
        await this.repository.SaveAsync();

    }
}
