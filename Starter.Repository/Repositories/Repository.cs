using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.Cosmos.Table;

using Starter.Framework.Entities;

namespace Starter.Repository.Repositories
{
    /// <summary>
    /// Implementation of the base Azure Table storage repository
    /// </summary>
    public class Repository
    {
        private readonly CloudStorageAccount _storageAccount;

        private readonly CloudTable _table;

        public Repository(ISettings settings, string tableName)
        {
            _storageAccount = CloudStorageAccount.Parse(settings.TableStorageConnectionString);

            var tableClient = _storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(tableName);

            _table.CreateIfNotExistsAsync();
        }

        private async Task<CloudTable> CreateTable(string tableName)
        {
            var tableClient = _storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            return table;
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