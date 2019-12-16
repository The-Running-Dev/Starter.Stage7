using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.SqlServer.Server;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Starter.Bootstrapper;
using Starter.Configuration.Entities;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Data.ViewModels;
using Starter.Framework.Clients;
using Starter.Framework.Extensions;

namespace Starter.Framework.Tests
{
    /// <summary>
    /// Base class for the Starter.Framework.Tests project
    /// </summary>
    public class TestsBase
    {
        protected List<Cat> Cats { get; set; }

        protected SqlDataRecord SqlDataRecord { get; set; }

        protected IApiClient ApiClient { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            CreateCatTestData();

            SqlDataRecord = new SqlDataRecord(
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier),
                new SqlMetaData("Name", SqlDbType.NVarChar, 20));

            var mockRestClient = new Mock<IRestClient>();
            var apiSettings = IocWrapper.Instance.GetService<IApiSettings>();

            // Setup the mock IRestClient
            mockRestClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<Cat>>(It.Is<RestRequest>((r) => r.Method == Method.GET), It.IsAny<CancellationToken>())).Returns(() =>
            {
                var response = new Mock<IRestResponse<IEnumerable<Cat>>>();

                response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
                response.Setup(_ => _.Data).Returns(Cats.AsEnumerable());

                return Task.FromResult(response.Object);
            });

            //mockRestClient.Setup(x => x.ExecuteTaskAsync(It.Is<RestRequest>((r) => r.Method == Method.POST), It.IsAny<CancellationToken>())).Returns((Cat entity) =>
            //{
            //    var response = new Mock<IRestResponse>();

            //    response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
            //    //response.Setup(_ => _.Data).Returns(Task.CompletedTask);

            //    Cats.Add(entity);

            //    return Task.FromResult(response.Object);
            //});

            //mockCatService.Setup(x => x.GetById(It.IsAny<Guid>()))
            //    .Returns((Guid id) => { return Task.FromResult(Cats.FirstOrDefault(x => x.Id == id)); });

            //mockCatService.Setup(x => x.Create(It.IsAny<Cat>()))
            //    .Returns((Cat entity) =>
            //    {
            //        Cats.Add(entity);

            //        return Task.CompletedTask;
            //    });

            //mockCatService.Setup(x => x.Update(It.IsAny<Cat>()))
            //    .Returns((Cat entity) =>
            //    {
            //        var existing = Cats.Find(x => x.Id == entity.Id);

            //        Cats.Remove(existing);
            //        Cats.Add(entity);

            //        return Task.CompletedTask;
            //    });

            //mockCatService.Setup(x => x.Delete(It.IsAny<Guid>()))
            //    .Returns((Guid id) =>
            //    {
            //        Cats.Remove(Cats.FirstOrDefault(x => x.Id == id));
            //        return Task.CompletedTask;
            //    });

            ApiClient = new ApiClient(mockRestClient.Object, apiSettings);
        }

        protected void CreateCatTestData()
        {
            Cats = new List<Cat>
            {
                new Cat("Widget", Ability.Eating),
                new Cat("Garfield",Ability.Engineering),
                new Cat("Mr. Boots", Ability.Lounging)
            };
        }
    }

    public enum TestEnum
    {
        [System.ComponentModel.Description("First Item")]
        FirstItem,
        SecondItem
    }
}