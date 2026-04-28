using Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Wrappers.Interfaces
{
    /// <summary>
    /// Interface containing the dependency injection for entity repository interfaces and repository classes.
    /// </summary>
    public interface IRepositoryWrapper
    {
        IEntityRepository Entity { get; }

        void Save();

        Task SaveAsync();
    }
}
