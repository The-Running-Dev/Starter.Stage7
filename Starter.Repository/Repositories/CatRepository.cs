using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.Cosmos.Table;

using Starter.Data.Entities;
using Starter.Data.Repositories;
using Starter.Configuration.Entities;

namespace Starter.Repository.Repositories
{
    /// <summary>
    /// Implementation of the CRUD operations for the Cat entity
    /// </summary>
    public class CatRepository : Repository, ICatRepository
    {
        public CatRepository(ISettings settings) : base(settings, settings.CatEntityTableName)
        {
        }

        public async Task<IEnumerable<Cat>> GetAll()
        {
            return await ExecuteQuery<Cat>();
        }

        public async Task<Cat> GetById(Guid id)
        {
            var query = new TableQuery<Cat>().Where(
                TableQuery.GenerateFilterCondition(
                    nameof(Cat.RowKey),
                    QueryComparisons.Equal,
                    id.ToString()));

            var entities = await ExecuteQuery<Cat>(query);

            return entities.FirstOrDefault();
        }

        public async Task<Cat> GetBySecondaryId(Guid id)
        {
            var query = new TableQuery<Cat>().Where(
                TableQuery.GenerateFilterConditionForGuid(
                    nameof(Cat.SecondaryId),
                    QueryComparisons.Equal,
                    id));

            var entities = await ExecuteQuery<Cat>(query);

            return entities.FirstOrDefault();
        }

        public async Task Create(Cat entity)
        {
            await ExecuteNonQuery(TableOperation.Insert(entity));
        }

        public async Task Update(Cat entity)
        {
            await ExecuteNonQuery(TableOperation.Replace(entity));
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);

            await ExecuteNonQuery(TableOperation.Delete(entity));
        }
    }
}