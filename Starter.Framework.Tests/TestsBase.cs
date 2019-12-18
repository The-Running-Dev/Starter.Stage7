using System;
using System.Data;

using NUnit.Framework;
using Microsoft.SqlServer.Server;

using Starter.Mocks;
using Starter.Bootstrapper;
using Starter.Framework.Clients;
using Starter.Configuration.Entities;

namespace Starter.Framework.Tests
{
    /// <summary>
    /// Implements tests setup
    /// </summary>
    public class TestsBase
    {
        protected SqlDataRecord SqlDataRecord { get; private set; }

        protected IApiClient ApiClient { get; private set; }

        protected RestClientMock RestClientMock { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            SqlDataRecord = new SqlDataRecord(
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier),
                new SqlMetaData("Name", SqlDbType.NVarChar, 20));

            var apiSettings = IocWrapper.Instance.GetService<IApiSettings>();

            RestClientMock = new RestClientMock();
            ApiClient = new ApiClient(RestClientMock.Instance, apiSettings);
        }
    }
}