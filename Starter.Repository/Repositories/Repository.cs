using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.Cosmos.Table;

using Starter.Configuration.Entities;

namespace Starter.Repository.Repositories
{
    /// <summary>
    /// Implementation of the base Azure Table storage repository
    /// </summary>
    public class Repository
    {
        private readonly CloudTable _table;

        public Repository(ISettings settings, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(settings.StorageAccountConnection);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            _table = tableClient.GetTableReference(tableName);
            _table.CreateIfNotExists();
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(TableQuery<T> query = null) where T : ITableEntity, new()
        {
            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query ?? new TableQuery<T>(), token);

                token = segment.ContinuationToken;

                items.AddRange(segment);
            } while (token != null);

            return items;
        }

        public async Task ExecuteNonQuery(TableOperation operation)
        {
            await _table.ExecuteAsync(operation);
        }
    }
}