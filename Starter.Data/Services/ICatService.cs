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
        /// <summary>
        /// Gets a list of all cats
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Cat>> GetAll();

        /// <summary>
        /// Cats a cat by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Cat> GetById(Guid id);

        /// <summary>
        /// Creates a new cat
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Create(Cat entity);

        /// <summary>
        /// Updates a cat
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(Cat entity);

        /// <summary>
        /// Deletes a cat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
    }
}