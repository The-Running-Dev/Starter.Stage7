using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Starter.Framework.Clients
{
    public interface IApiClient
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<T> GetById<T>(Guid id);

        Task Create<T>(T entity);

        Task Update<T>(T entity);

        Task Delete(Guid id);
    }
}