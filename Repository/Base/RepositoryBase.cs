using Contracts.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base
{
    /// <summary>
    /// Base repository class for implementation with Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">Entity model class</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        protected RepositoryBase() { }

        /// <summary>
        /// Method that retrieves all entity data.
        /// </summary>
        /// <returns>Linq Query object</returns>
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Method that retrieves entity data based on a condition.
        /// </summary>
        /// <param name="expression">Condition expression</param>
        /// <returns>Linq Query object</returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Method that allows inserting an object with entity data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Method that allows updating entity object data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        /// <summary>
        /// Method that allows deleting an object's entity data.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Asynchronous method that allows saving CRUD changes.
        /// </summary>
        public async Task SaveAsync()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }
    }
}
