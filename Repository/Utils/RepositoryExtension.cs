using Entities.Utils.Paged;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Utils
{
    // Using from: https://www.codingame.com/playgrounds/5363/paging-with-entity-framework-core
    // And https://gunnarpeipman.com/net/ef-core-paging/

    /// <summary>
    /// Clase de extensión para el uso dentro de las clases del repositorio
    /// </summary>
    public static class RepositoryExtension
    {
        /// <summary>
        /// Funcion que permite obtener un objeto de Páginación para el repositorio que lo necesite
        /// </summary>
        /// <typeparam name="T">Clase de Entidad de Repositorio</typeparam>
        /// <param name="query">Query, definida como Queryable</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <returns>Objeto de Paginación con lista de objetos</returns>
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).AsEnumerable().ToList();

            return result;
        }

        /// <summary>
        /// Funcion que permite obtener una lista de objetos páginada para el repositorio que lo necesite
        /// </summary>
        /// <typeparam name="T">Clase de Entidad de Repositorio</typeparam>
        /// <param name="query">Query, definida como Queryable</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <returns>Lista de objetos páginada</returns>
        public static IEnumerable<T> GetPagedList<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var skip = (page - 1) * pageSize;
            var results = query.Skip(skip).Take(pageSize).AsEnumerable().ToList();

            return results;
        }

        /// <summary>
        /// Funcion asíncrona que permite obtener un objeto de Páginación para el repositorio que lo necesite
        /// </summary>
        /// <typeparam name="T">Clase de Entidad de Repositorio</typeparam>
        /// <param name="query">Query, definida como Queryable</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <returns>Objeto de Paginación con lista de objetos</returns>
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            var results = query.Skip(skip).Take(pageSize).AsEnumerable();
            result.Results = await Task.FromResult(results.ToList());

            return result;
        }

        /// <summary>
        /// Funcion Asíncrona que permite obtener una lista de objetos páginada para el repositorio que lo necesite
        /// </summary>
        /// <typeparam name="T">Clase de Entidad de Repositorio</typeparam>
        /// <param name="query">Query, definida como Queryable</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <returns>Lista de objetos páginada</returns>
        public static async Task<IEnumerable<T>> GetPagedListAsync<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var skip = (page - 1) * pageSize;
            var resultsEnumerable = query.Skip(skip).Take(pageSize).AsEnumerable();
            var results = await Task.FromResult(resultsEnumerable.ToList());

            return results;
        }
    }
}
