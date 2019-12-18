using NUnit.Framework;
using Microsoft.Azure.Cosmos.Table;

using Starter.Mocks;
using Starter.Bootstrapper;
using Starter.Data.Repositories;
using Starter.Configuration.Entities;

namespace Starter.Repository.Tests
{
    /// <summary>
    /// Implements tests setup
    /// </summary>
    public class TestsBase
    {
        protected ICatRepository CatRepository { get; set; }

        protected CloudStorageAccount StorageAccount { get; set; }

        public CloudTable CatsTable { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            CatRepository = IocWrapper.Instance.GetService<ICatRepository>();
            
            var settings = IocWrapper.Instance.GetService<ISettings>();
            StorageAccount = CloudStorageAccount.Parse(settings.StorageAccountConnection);

            var tableClient = StorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CatsTable = tableClient.GetTableReference(settings.CatEntityTableName);

            foreach (var cat in TestData.Cats)
            {
                CatsTable.Execute(TableOperation.Insert(cat));
            }
        }
    }
}