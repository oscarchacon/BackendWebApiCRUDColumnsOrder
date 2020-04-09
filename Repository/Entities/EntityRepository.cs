using Contracts.Entities;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged;
using Entities.Utils.Paged.Interfaces;
using Repository.Base;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Entities
{
    /// <summary>
    /// Clase de repositorio de la Entidad, la cual contiene la implementación del CRUD
    /// </summary>
    public class EntityRepository : RepositoryBase<Entity>, IEntityRepository
    {
        public EntityRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        /// <summary>
        /// Método que permite obtener de la base de datos una lista con todos los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Lista de Datos de Entidad</returns>
        public IEnumerable<Entity> GetAll(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false)
        {
            var entitiesFind = this.FindAll();
            if (columnName != null && !columnName.Equals(string.Empty))
            {
                if (EntityProperties.ContainsPropertyName(typeof(Entity), columnName))
                {
                    if (!orderDesc)
                    {
                        entitiesFind = entitiesFind.CustomOrderBy(columnName);
                    }
                    else
                    {
                        entitiesFind = entitiesFind.CustomOrderByDescending(columnName);
                    }
                }
            }
            if (page.HasValue && pageSize.HasValue)
            {
                return entitiesFind.GetPagedList(page.Value, pageSize.Value);
            }

            return entitiesFind.AsEnumerable().ToList();
        }

        /// <summary>
        /// Método que permite obtener de la base de datos un objeto de Paginación con la lista de todos los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Objeto de Paginación con la Lista de Datos de Entidad</returns>
        public IPagedResult<Entity> GetAllPaged(int? page = null, int? pageSize = null, string columnName = null, bool orderDesc = false)
        {
            var entitiesFind = this.FindAll();
            if (columnName != null && !columnName.Equals(string.Empty))
            {
                if (EntityProperties.ContainsPropertyName(typeof(Entity), columnName))
                {
                    if (!orderDesc)
                    {
                        entitiesFind = entitiesFind.CustomOrderBy(columnName);
                    }
                    else
                    {
                        entitiesFind = entitiesFind.CustomOrderByDescending(columnName);
                    }
                }
            }
            if (page.HasValue && pageSize.HasValue)
            {
                return entitiesFind.GetPaged(page.Value, pageSize.Value);
            }

            return new PagedResult<Entity>
            {
                RowCount = entitiesFind.Count(),
                Results = entitiesFind.AsEnumerable().ToList()
            };
        }
        /// <summary>
        /// Método que permite obtener de la base de datos una Entidad por medio de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Empresa</returns>
        public Entity GetById(Guid entityId)
        {
            var entityFind = this.FindByCondition(entity => entity.Id.Equals(entityId));
            return entityFind.AsEnumerable().DefaultIfEmpty(new Entity()).FirstOrDefault();
        }

        /// <summary>
        /// Método que permite crear un registro de una Entidad en la base de datos
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        public void CreateEntity(Entity entity)
        {
            entity.Id = new Guid();
            this.Create(entity);
        }

        /// <summary>
        /// Método que permite hacer una actualización en la base de datos de una Entidad
        /// </summary>
        /// <param name="dbEntity">Objeto Entidad con datos provenientes de la base de datos</param>
        /// <param name="entity">Objeto Entidad con los datos a actualizar</param>
        public void UpdateEntity(Entity dbEntity, Entity entity)
        {
            dbEntity.Map(entity);
            this.Update(dbEntity);
        }

        /// <summary>
        /// Método que permite eliminar un registro de la base de datos de una Entidad
        /// </summary>
        /// <param name="entity">Objeto Entidad</param>
        public void DeleteEntity(Entity entity)
        {
            this.Delete(entity);
        }
    }
}
