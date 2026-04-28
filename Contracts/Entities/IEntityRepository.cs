using Contracts.Interfaces;
using Entities.Models;
using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Entities
{
    /// <summary>
    /// Interface for the implementation within the Entity Repository.
    /// </summary>
    public interface IEntityRepository : IRepositoryBase<Entity>
    {
        /// <summary>
        /// Implementation method that allows retrieving a list of all entity data from the database.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <returns>List of entity data</returns>
        IEnumerable<Entity> GetAll(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false);

        /// <summary>
        /// Implementation method that allows retrieving a list of all entity data from the database asynchronously.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of entity data</returns>
        Task<IEnumerable<Entity>> GetAllAsync(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Implementation method that allows retrieving a pagination object with the list of all entity data.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <returns>Pagination object with the list of entity data</returns>
        IPagedResult<Entity> GetAllPaged(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false);

        /// <summary>
        /// Implementation method that allows retrieving a pagination object with the list of all entity data asynchronously.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="columnName">Column name to sort by</param>
        /// <param name="orderDesc">Descending sort boolean</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Pagination object with the list of entity data</returns>
        Task<IPagedResult<Entity>> GetAllPagedAsync(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Implementation method that allows retrieving an entity from the database using its Id.
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns>Entity object</returns>
        Entity GetById(Guid entityId);

        /// <summary>
        /// Implementation method that allows retrieving an entity from the database using its Id asynchronously.
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entity object</returns>
        Task<Entity> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Implementation method that allows creating an entity record in the database.
        /// </summary>
        /// <param name="entity">Entity object</param>
        void CreateEntity(Entity entity);

        /// <summary>
        /// Implementation method that allows creating an entity record in the database asynchronously.
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task CreateEntityAsync(Entity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Implementation method that allows updating data for an entity in the database.
        /// </summary>
        /// <param name="dbEntity">Entity object with data coming from the database</param>
        /// <param name="entity">Entity object with the data to update</param>
        void UpdateEntity(Entity dbEntity, Entity entity);

        /// <summary>
        /// Implementation method that allows deleting an entity record from the database.
        /// </summary>
        /// <param name="entity">Entity object</param>
        void DeleteEntity(Entity entity);
    }
}
