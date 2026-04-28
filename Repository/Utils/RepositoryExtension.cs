using Entities.Utils.Paged;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Utils
{
    /// <summary>
    /// Extension class for use within repository classes.
    /// </summary>
    public static class RepositoryExtension
    {
        /// <summary>
        /// Function that allows obtaining a pagination object for the repository that needs it.
        /// </summary>
        /// <typeparam name="T">Repository entity class</typeparam>
        /// <param name="query">Query, defined as Queryable</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <returns>Pagination object with list of objects</returns>
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
        /// Asynchronous function that allows obtaining a pagination object for the repository that needs it.
        /// </summary>
        /// <typeparam name="T">Repository entity class</typeparam>
        /// <param name="query">Query, defined as Queryable</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Pagination object with list of objects</returns>
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                          int page, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync(cancellationToken)
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            var results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
            result.Results = results;

            return result;
        }

        /// <summary>
        /// Function that allows obtaining a paged list of objects for the repository that needs it.
        /// </summary>
        /// <typeparam name="T">Repository entity class</typeparam>
        /// <param name="query">Query, defined as Queryable</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <returns>Paged list of objects</returns>
        public static IEnumerable<T> GetPagedList<T>(this IQueryable<T> query,
                                          int page, int pageSize) where T : class
        {
            var skip = (page - 1) * pageSize;
            var results = query.Skip(skip).Take(pageSize).AsEnumerable().ToList();

            return results;
        }

        /// <summary>
        /// Asynchronous function that allows obtaining a paged list of objects for the repository that needs it.
        /// </summary>
        /// <typeparam name="T">Repository entity class</typeparam>
        /// <param name="query">Query, defined as Queryable</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged list of objects</returns>
        public static async Task<IEnumerable<T>> GetPagedListAsync<T>(this IQueryable<T> query,
                                          int page, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            var skip = (page - 1) * pageSize;
            var results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return results;
        }

        /// <summary>
        /// Asynchronous function that allows obtaining a pagination object for the repository that needs it.
        /// </summary>
        /// <typeparam name="T">Repository entity class</typeparam>
        /// <param name="query">Query, defined as Queryable</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Elements per page</param>
        /// <returns>Pagination object with list of objects</returns>
        public static async Task<PagedResult<T>> GetPagedAsAsync<T>(this IQueryable<T> query,
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
    }
}
