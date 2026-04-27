using AutoMapper;
using Entities.Helpers.Entities;
using BusinesRules.Exceptions;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged.Interfaces;
using Repository.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.DTO;

namespace BusinesRules.Entities;

/// <summary>
/// Clase de Reglas de Negocios para Entidades
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
    /// Función que permite obtener una Lista con todas los datos de Entidad
    /// </summary>
    /// <param name="page">Página Actual</param>
    /// <param name="pageSize">Elementos por Página</param>
    /// <param name="columnName">Nombre de columna a Ordenar</param>
    /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
    /// <returns>Lista de Datos de Entidad</returns>
    public async Task<IEnumerable<Entity>> GetAllEntities(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
    {

        var entities = await this.repository.Entity.GetAllAsync(page, pageSize, columnName, orderDesc);
        return entities;

    }

    /// <summary>
    /// Función que permite obtener un Objeto de Paginación con Lista con todas los datos de Entidad
    /// </summary>
    /// <param name="page">Página Actual</param>
    /// <param name="pageSize">Elementos por Página</param>
    /// <param name="columnName">Nombre de columna a Ordenar</param>
    /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
    /// <returns>Objeto de Paginación con Lista de Datos d Entidad</returns>
    public async Task<IPagedResult<Entity>> GetAllEntitiesPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
    {

        var entities = await this.repository.Entity.GetAllPagedAsync(page, pageSize, columnName, orderDesc);
        return entities;

    }

    /// <summary>
    /// Función que busca una Entidad por su Id de forma asíncrona
    /// </summary>
    /// <param name="companyId">Id de Entidad</param>
    /// <returns>Objeto Entidad</returns>
    public async Task<Entity> GetEntityById(Guid entityId)
    {

        var company = await this.repository.Entity.GetByIdAsync(entityId);

        return company;

    }

    /// <summary>
    /// Función que crea una nueva Entidad
    /// </summary>
    /// <param name="entityRegister">Objeto Entidad para Registrar</param>
    /// <returns>Objeto Entidad creado</returns>
    public async Task CreateEntity(Entity entity)
    {

        entity.RegisterDate = DateTime.UtcNow;
        await this.repository.Entity.CreateEntityAsync(entity);
        await this.repository.SaveAsync();

    }

    /// <summary>
    /// Función que actualiza los datos de una Entidad
    /// </summary>
    /// <param name="entityId">Id de Entidad</param>
    /// <param name="entityUpdated">Objeto Entidad con los nuevos datos</param>
    /// <returns>Booleano si se realizó la acción</returns>
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
    /// Función que permite eliminar una Entidad a partir de su Id
    /// </summary>
    /// <param name="entityId">Id de Entidad</param>
    /// <returns>Objeto Booleano si se realizó la acción</returns>
    public async Task DeleteEntity(Guid entityId)
    {

        var dbEntity = this.repository.Entity.GetById(entityId);
        if (dbEntity.IsEmptyObject()) { throw new NotFoundException("Entity not found"); }


        this.repository.Entity.DeleteEntity(dbEntity);
        await this.repository.SaveAsync();

    }
}
