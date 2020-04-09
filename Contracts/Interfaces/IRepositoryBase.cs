using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    /// <summary>
    /// Interface de Base para ser implementada con los métodos CRUD usados para las clases de entidades
    /// </summary>
    /// <typeparam name="T">Clase Parametro</typeparam>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Método de implementación que obtiene todos los datos de la entidad
        /// </summary>
        /// <returns>Objeto Query Linq</returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// Método de implementación que obtiene los datos de la entidad por medio de una condición
        /// </summary>
        /// <param name="expression">Expresión de Condición</param>
        /// <returns>Objeto Query Linq</returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Método de implementación que permite insertar un objeto con los datos de la entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        void Create(T entity);

        /// <summary>
        /// Método de implementación que permite actualizar los datos del objeto de entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        void Update(T entity);

        /// <summary>
        /// Método de implementación que permite eliminar los datos de un objeto de entidad
        /// </summary>
        /// <param name="entity">Objeto de Entidad</param>
        void Delete(T entity);

        /// <summary>
        /// Método Asíncrono de implementación que permite guardar los cambios CRUD
        /// </summary>
        Task SaveAsync();
    }
}
