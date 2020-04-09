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
    /// Clase de Repositorio Base, para implemetación con Entity Framework Core
    /// </summary>
    /// <typeparam name="T">Clase de Entidad del Modelo</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        protected RepositoryBase() { }

        /// <summary>
        /// Método que obtiene todos los datos de la entidad
        /// </summary>
        /// <returns>Objeto Query Linq</returns>
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Método que obtiene los datos de la entidad por medio de una condición
        /// </summary>
        /// <param name="expression">Expresión de Condición</param>
        /// <returns>Objeto Query Linq</returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Método que permite insertar un objeto con los datos de la entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Método que permite actualizar los datos del objeto de entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        /// <summary>
        /// Método que permite eliminar los datos de un objeto de entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Método Asíncrono que permite guardar los cambios CRUD
        /// </summary>
        public async Task SaveAsync()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }
    }
}
