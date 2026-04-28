using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    /// <summary>
    /// Base interface to be implemented with CRUD methods used for entity classes.
    /// </summary>
    /// <typeparam name="T">Parameter class</typeparam>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Implementation method that obtains all entity data.
        /// </summary>
        /// <returns>Linq Query object</returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// Implementation method that obtains entity data based on a condition.
        /// </summary>
        /// <param name="expression">Condition expression</param>
        /// <returns>Linq Query object</returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Implementation method that allows inserting an object with entity data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        void Create(T entity);

        /// <summary>
        /// Asynchronous method that allows inserting an object with entity data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Implementation method that allows updating entity object data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        void Update(T entity);

        /// <summary>
        /// Implementation method that allows deleting an object's entity data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        void Delete(T entity);

        /// <summary>
        /// Asynchronous implementation method that allows saving CRUD changes.
        /// </summary>
        Task SaveAsync();
    }
}
