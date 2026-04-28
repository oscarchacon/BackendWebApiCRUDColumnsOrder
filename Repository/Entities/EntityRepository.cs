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
    /// Entity repository class containing the CRUD implementation.
    /// </summary>
    public class EntityRepository : RepositoryBase<Entity>, IEntityRepository
    {
        public EntityRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        /// <summary>
        /// Method that retrieves a list of all entity data from the database.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <returns>List of entity data</returns>
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
        /// Method that retrieves a pagination object with the list of all entity data asynchronously from the database.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Pagination object with the list of entity data</returns>
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
        /// Method that retrieves an entity from the database using its Id.
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns>Entity object</returns>
        public async Task<Entity> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entityFind = this.FindByCondition(entity => entity.Id.Equals(entityId));
            var entity = await entityFind.FirstOrDefaultAsync(cancellationToken);
            return entity ?? new Entity();
        }

        /// <summary>
        /// Method that creates an entity record in the database asynchronously.
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task CreateEntityAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            entity.Id = new Guid();
            await this.CreateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Method that performs an update in the database for an entity.
        /// </summary>
        /// <param name="dbEntity">Entity object with data coming from the database</param>
        /// <param name="entity">Entity object with the data to update</param>
        public void UpdateEntity(Entity dbEntity, Entity entity)
        {
            dbEntity.Map(entity);
            this.Update(dbEntity);
        }

        /// <summary>
        /// Method that allows deleting an entity record from the database.
        /// </summary>
        /// <param name="entity">Entity object</param>
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
        /// Method that retrieves an entity from the database using its Id.
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns>Entity object</returns>
        public Entity GetById(Guid entityId)
        {
            var entityFind = this.FindByCondition(entity => entity.Id.Equals(entityId));
            return entityFind.AsEnumerable().DefaultIfEmpty(new Entity()).FirstOrDefault();
        }

        /// <summary>
        /// Method that creates an entity record in the database.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void CreateEntity(Entity entity)
        {
            entity.Id = new Guid();
            this.Create(entity);
        }
    }
}
