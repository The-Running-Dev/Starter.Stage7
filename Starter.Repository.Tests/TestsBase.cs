using System.Collections.Generic;

using NUnit.Framework;
using Microsoft.Azure.Cosmos.Table;

using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.Data.Repositories;
using Starter.Framework.Entities;

namespace Starter.Repository.Tests
{
    /// <summary>
    /// Base class for the Starter.Repository.Tests project
    /// </summary>
    public class TestsBase
    {
        protected ICatRepository CatRepository { get; set; }

        protected List<Cat> Cats { get; set; }

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
        }

        /// <summary>
        /// Cleans up created records
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CatsTable.DeleteIfExists();
        }

        /// <summary>
        /// Creates tests data
        /// </summary>
        protected void CreateTestData()
        {
            Cats = new List<Cat>
            {
                new Cat("Widget", Ability.Eating),
                new Cat("Garfield", Ability.Engineering),
                new Cat("Mr. Boots", Ability.Lounging)
            };

            foreach (var cat in Cats)
            {
                CatsTable.Execute(TableOperation.Insert(cat));
            }
        }
    }
}