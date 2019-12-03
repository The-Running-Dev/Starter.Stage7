using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Starter.Data.Entities;

namespace Starter.Data.Services
{
    /// <summary>
    /// Defines the contract for the Cat related business logic
    /// </summary>
    public interface ICatService
    {
        Task<IEnumerable<Cat>> GetAll();

        Task<Cat> GetById(Guid id);

        Task Create(Cat entity);

        Task Update(Cat entity);

        Task Delete(Guid id);
    }
}