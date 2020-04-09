using Entities.Models;
using Entities.Utils.Paged.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Entities
{
    /// <summary>
    /// Interface para la implementación dentro del Repositorio de Entidad
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// Método de implementación que permite obtener de la base de datos una lista con todos los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Lista de Datos de Entidad</returns>
        IEnumerable<Entity> GetAll(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false);

        /// <summary>
        /// Método de implementación que permite obtener de la base de datos un objeto de Paginación con la lista de todos los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Objeto de Paginación con la Lista de Datos de Entidad</returns>
        IPagedResult<Entity> GetAllPaged(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false);

        /// <summary>
        /// Método de implementación que permite obtener de la base de datos una Entidad por medio de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Empresa</returns>
        Entity GetById(Guid entityId);

        /// <summary>
        /// Método de implementación que permite crear un registro de una Entidad en la base de datos
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        void CreateEntity(Entity entity);

        /// <summary>
        /// Método de implementación que permite hacer una actualización en la base de datos de una Entidad
        /// </summary>
        /// <param name="dbEntity">Objeto Entidad con datos provenientes de la base de datos</param>
        /// <param name="entity">Objeto Entidad con los datos a actualizar</param>
        void UpdateEntity(Entity dbEntity, Entity entity);

        /// <summary>
        /// Método de implementación que permite eliminar un registro de la base de datos de una Entidad
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        void DeleteEntity(Entity entity);
    }
}
