using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Repository.Utils
{
    /// <summary>
    /// Static extension class for Entity Framework.
    /// </summary>
    public static class EFExtensions
    {

        /// <summary>
        /// Function that allows having an asynchronous list without errors.
        /// </summary>
        /// <typeparam name="TSource">Parameter class</typeparam>
        /// <param name="source">Source object with parameter class</param>
        /// <returns>Asynchronous task with safe list</returns>
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }

        /// <summary>
        /// Method that allows sorting a query object by a property name in ascending order.
        /// </summary>
        /// <typeparam name="TSource">Parameter class</typeparam>
        /// <param name="query">Linq query object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Sorted query object</returns>
        public static IOrderedQueryable<TSource> CustomOrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            var properties = entityType.GetProperties();
            var propertyInfo = Array.Find(properties, propertyClass => propertyClass.Name.ToLower().Equals(propertyName.ToLower()));
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        /// Method that allows sorting a query object by a property name in descending order.
        /// </summary>
        /// <typeparam name="TSource">Parameter class</typeparam>
        /// <param name="query">Linq query object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Sorted query object</returns>
        public static IOrderedQueryable<TSource> CustomOrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            var properties = entityType.GetProperties();
            var propertyInfo = Array.Find(properties, propertyClass => propertyClass.Name.ToLower().Equals(propertyName.ToLower()));
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}
