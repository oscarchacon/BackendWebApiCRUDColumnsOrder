using Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Wrappers.Interfaces
{
    /// <summary>
    /// Interface que contiene la inyección de dependencias con las interfaces de repositorios con las clases de repositorio
    /// </summary>
    public interface IRepositoryWrapper
    {
        IEntityRepository Entity { get; }

        void Save();

        Task SaveAsync();
    }
}
