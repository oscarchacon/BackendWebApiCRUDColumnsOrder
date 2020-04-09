using AutoMapper;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged.Interfaces;
using Repository.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinesRules.Entities
{
    /// <summary>
    /// Clase de Reglas de Negocios para Entidades
    /// </summary>
    public class EntitiesBR
    {
        private readonly IRepositoryWrapper repository;

        public EntitiesBR(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Función que permite obtener una Lista con todas los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Lista de Datos de Entidad</returns>
        public IEnumerable<Entity> GetAllEntities(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = this.repository.Entity.GetAll(page, pageSize, columnName, orderDesc);
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que permite obtener un Objeto de Paginación con Lista con todas los datos de Entidad
        /// </summary>
        /// <param name="page">Página Actual</param>
        /// <param name="pageSize">Elementos por Página</param>
        /// <param name="columnName">Nombre de columna a Ordenar</param>
        /// <param name="orderDesc">Booleano de ordenamiento descendente</param>
        /// <returns>Objeto de Paginación con Lista de Datos d Entidad</returns>
        public IPagedResult<Entity> GetAllEntitiesPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = this.repository.Entity.GetAllPaged(page, pageSize, columnName, orderDesc);
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que busca una Entidad por su Id
        /// </summary>
        /// <param name="companyId">Id de Entidad</param>
        /// <returns>Objeto Entidad</returns>
        public Entity GetEntityById(Guid entityId)
        {
            try
            {
                var company = this.repository.Entity.GetById(entityId);

                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que crea una nueva Entidad
        /// </summary>
        /// <param name="entityRegister">Objeto Entidad para Registrar</param>
        /// <returns>Objeto Entidad creado</returns>
        public void CreateEntity(Entity entity)
        {
            try
            {
                entity.RegisterDate = DateTime.UtcNow;
                this.repository.Entity.CreateEntity(entity);
                this.repository.Save();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que actualiza los datos de una Entidad
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <param name="entityUpdated">Objeto Entidad con los nuevos datos</param>
        /// <returns>Booleano si se realizó la acción</returns>
        public bool UpdateEntity(Guid entityId, Entity entityUpdated)
        {
            try
            {
                var dbEntity = this.repository.Entity.GetById(entityId);
                if (dbEntity.IsEmptyObject()) { return false; }

                this.repository.Entity.UpdateEntity(dbEntity, entityUpdated);
                this.repository.Save();

                entityUpdated.Id = entityId;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que permite eliminar una Entidad a partir de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Booleano si se realizó la acción</returns>
        public object DeleteEntity(Guid entityId)
        {
            try
            {
                var dbEntity = this.repository.Entity.GetById(entityId);
                if (dbEntity.IsEmptyObject()) { return null; }


                this.repository.Entity.DeleteEntity(dbEntity);
                this.repository.Save();

                return true;
            }
            catch (ArgumentNullException anex)
            {
                throw new Exception(anex.Message);
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }
    }
}
