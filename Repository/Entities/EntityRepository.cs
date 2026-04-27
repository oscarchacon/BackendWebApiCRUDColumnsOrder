using Contracts.Entities;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged;
using Entities.Utils.Paged.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Entities
{
    /// <summary>
    /// Clase de repositorio de la Entidad, la cual contiene la implementación del CRUD
    /// </summary>
    public class EntityRepository : RepositoryBase<Entity>, IEntityRepository
    {
        public EntityRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        /// <summary>
        /// Método que permite obtener de la base de datos una lista con todos los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Lista de Datos de Entidad</returns>
        public async Task<IEnumerable<Entity>> GetAllAsync(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false, CancellationToken cancellationToken = default)
        {
            var entitiesFind = this.FindAll();
            if (columnName != null && !columnName.Equals(string.Empty))
            {
                if (EntityProperties.ContainsPropertyName(typeof(Entity), columnName))
                {
                    if (!orderDesc)
                    {
                        entitiesFind = entitiesFind.CustomOrderBy(columnName);
                    }
                    else
                    {
                        entitiesFind = entitiesFind.CustomOrderByDescending(columnName);
                    }
                }
            }
            if (page.HasValue && pageSize.HasValue)
            {
                return await entitiesFind.GetPagedListAsync(page.Value, pageSize.Value, cancellationToken);
            }

            return await entitiesFind.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Método que permite obtener de la base de datos un objeto de Paginación con la lista de todos los datos de Entidad de forma asíncrona
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Objeto de Paginación con la Lista de Datos de Entidad</returns>
        public async Task<IPagedResult<Entity>> GetAllPagedAsync(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false, CancellationToken cancellationToken = default)
        {
            var entitiesFind = this.FindAll();
            if (columnName != null && !columnName.Equals(string.Empty))
            {
                if (EntityProperties.ContainsPropertyName(typeof(Entity), columnName))
                {
                    if (!orderDesc)
                    {
                        entitiesFind = entitiesFind.CustomOrderBy(columnName);
                    }
                    else
                    {
                        entitiesFind = entitiesFind.CustomOrderByDescending(columnName);
                    }
                }
            }
            if (page.HasValue && pageSize.HasValue)
            {
                return await entitiesFind.GetPagedAsync(page.Value, pageSize.Value, cancellationToken);
            }

            return new PagedResult<Entity>
            {
                RowCount = await entitiesFind.CountAsync(cancellationToken),
                Results = await entitiesFind.ToListAsync(cancellationToken)
            };
        }
        /// <summary>
        /// Método que permite obtener de la base de datos una Entidad por medio de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Entidad</returns>
        public async Task<Entity> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entityFind = this.FindByCondition(entity => entity.Id.Equals(entityId));
            var entity = await entityFind.FirstOrDefaultAsync(cancellationToken);
            return entity ?? new Entity();
        }

        /// <summary>
        /// Método que permite crear un registro de una Entidad en la base de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        public async Task CreateEntityAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            entity.Id = new Guid();
            await this.CreateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Método que permite hacer una actualización en la base de datos de una Entidad
        /// </summary>
        /// <param name="dbEntity">Objeto Entidad con datos provenientes de la base de datos</param>
        /// <param name="entity">Objeto Entidad con los datos a actualizar</param>
        public void UpdateEntity(Entity dbEntity, Entity entity)
        {
            dbEntity.Map(entity);
            this.Update(dbEntity);
        }

        /// <summary>
        /// Método que permite eliminar un registro de la base de datos de una Entidad
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        public void DeleteEntity(Entity entity)
        {
            this.Delete(entity);
        }

        public IEnumerable<Entity> GetAll(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false)
        {
            throw new NotImplementedException();
        }

        public IPagedResult<Entity> GetAllPaged(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que permite obtener de la base de datos una Entidad por medio de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Entidad</returns>
        public Entity GetById(Guid entityId)
        {
            var entityFind = this.FindByCondition(entity => entity.Id.Equals(entityId));
            return entityFind.AsEnumerable().DefaultIfEmpty(new Entity()).FirstOrDefault();
        }

        /// <summary>
        /// Método que permite crear un registro de una Entidad en la base de datos
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        public void CreateEntity(Entity entity)
        {
            entity.Id = new Guid();
            this.Create(entity);
        }
    }
}
