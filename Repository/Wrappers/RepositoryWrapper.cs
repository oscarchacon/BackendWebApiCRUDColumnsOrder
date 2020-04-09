using Contracts.Entities;
using Entities;
using Repository.Entities;
using Repository.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Wrappers
{
    /// <summary>
    /// Clase que contiene las dependencias de los repositorios de las entidades
    /// </summary>
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext repositoryContext;
        private IEntityRepository entity;

        public IEntityRepository Entity 
        {
            get
            {
                if (this.entity == null)
                {
                    this.entity = new EntityRepository(this.repositoryContext);
                }
                return this.entity;
            }
        }

        public void Save()
        {
            this.repositoryContext.SaveChanges();
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }
    }
}
