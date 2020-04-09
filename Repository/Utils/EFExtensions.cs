using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Repository.Utils
{
    // Using from: https://expertcodeblog.wordpress.com/2018/02/19/net-core-2-0-resolve-error-the-source-iqueryable-doesnt-implement-iasyncenumerable/
    /// <summary>
    /// Clase de Estatica de Extensión para Entity Framework
    /// </summary>
    public static class EFExtensions
    {

        /// <summary>
        /// Función que permite tener una lista asíncronica que no tenga errores
        /// </summary>
        /// <typeparam name="TSource">Clase de Parámetro</typeparam>
        /// <param name="source">Objeto de Procedencia con Clase de Parámetro</param>
        /// <returns>Tarea Asíncrona con Lista segura</returns>
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }

        // Code From: http://www.chrisloves.net/blogs/entity-framework-order-by-column-name-as-string/
        // Author: Chris Bullard
        /// <summary>
        /// Método  que permite ordenar un objeto query con el nombre de una propiedad de manera ascendente
        /// </summary>
        /// <typeparam name="TSource">Clase de Parametro</typeparam>
        /// <param name="query">Objeto de tipo query Linq</param>
        /// <param name="propertyName">Nombre de propiedad</param>
        /// <returns>Objeto Query Ordenado</returns>
        public static IOrderedQueryable<TSource> CustomOrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var properties = entityType.GetProperties();
            var propertyInfo = Array.Find(properties, propertyClass => propertyClass.Name.ToLower().Equals(propertyName.ToLower()));
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Call query.OrderBy(selector), with query and selector: x=> x.PropName
            // Note that we pass the selector as Expression to the method and we don't compile it.
            // By doing so EF can extract "order by" columns and generate SQL for it
            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        /// Método  que permite ordenar un objeto query con el nombre de una propiedad de manera descendente
        /// </summary>
        /// <typeparam name="TSource">Clase de Parametro</typeparam>
        /// <param name="query">Objeto de tipo query Linq</param>
        /// <param name="propertyName">Nombre de propiedad</param>
        /// <returns>Objeto Query Ordenado</returns>
        public static IOrderedQueryable<TSource> CustomOrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var properties = entityType.GetProperties();
            var propertyInfo = Array.Find(properties, propertyClass => propertyClass.Name.ToLower().Equals(propertyName.ToLower()));
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            //The linq's OrderByDescending<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Call query.OrderByDescending(selector), with query and selector: x=> x.PropName
            // Note that we pass the selector as Expression to the method and we don't compile it.
            // By doing so EF can extract "order by" columns and generate SQL for it
            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}
