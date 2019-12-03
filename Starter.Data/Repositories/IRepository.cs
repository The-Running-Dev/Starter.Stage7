using System;

using Starter.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Starter.Data.Repositories
{
    /// <summary>
    /// Defines the contract for base repository functions
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(Guid id);

        Task<TEntity> GetBySecondaryId(Guid id);

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(Guid id);
    }
}