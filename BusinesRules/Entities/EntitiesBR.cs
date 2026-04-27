using AutoMapper;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged.Interfaces;
using Repository.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<Entity>> GetAllEntities(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = await this.repository.Entity.GetAllAsync(page, pageSize, columnName, orderDesc);
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
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
        public async Task<IPagedResult<Entity>> GetAllEntitiesPaged(int? page, int? pageSize, string columnName = null, bool orderDesc = false)
        {
            try
            {
                var entities = await this.repository.Entity.GetAllPagedAsync(page, pageSize, columnName, orderDesc);
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Función que busca una Entidad por su Id de forma asíncrona
        /// </summary>
        /// <param name="companyId">Id de Entidad</param>
        /// <returns>Objeto Entidad</returns>
        public async Task<Entity> GetEntityById(Guid entityId)
        {
            try
            {
                var company = await this.repository.Entity.GetByIdAsync(entityId);

                return company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Función que crea una nueva Entidad
        /// </summary>
        /// <param name="entityRegister">Objeto Entidad para Registrar</param>
        /// <returns>Objeto Entidad creado</returns>
        public async Task CreateEntity(Entity entity)
        {
            try
            {
                entity.RegisterDate = DateTime.UtcNow;
                await this.repository.Entity.CreateEntityAsync(entity);
                await this.repository.SaveAsync();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Función que actualiza los datos de una Entidad
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <param name="entityUpdated">Objeto Entidad con los nuevos datos</param>
        /// <returns>Booleano si se realizó la acción</returns>
        public async Task<bool> UpdateEntity(Guid entityId, Entity entityUpdated)
        {
            try
            {
                var dbEntity = await this.repository.Entity.GetByIdAsync(entityId);
                if (dbEntity.IsEmptyObject()) { throw new NotFoundException("Entity not found"); }

                this.repository.Entity.UpdateEntity(dbEntity, entityUpdated);
                await this.repository.SaveAsync();

                entityUpdated.Id = entityId;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Función que permite eliminar una Entidad a partir de su Id
        /// </summary>
        /// <param name="entityId">Id de Entidad</param>
        /// <returns>Objeto Booleano si se realizó la acción</returns>
        public async Task<object> DeleteEntity(Guid entityId)
        {
            try
            {
                var dbEntity = this.repository.Entity.GetById(entityId);
                if (dbEntity.IsEmptyObject()) { return null; }


                this.repository.Entity.DeleteEntity(dbEntity);
                await this.repository.SaveAsync();

                return true;
            }
            catch (ArgumentNullException anex)
            {
                throw anex;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
