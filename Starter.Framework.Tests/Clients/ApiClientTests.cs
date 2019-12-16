using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;
using Moq;
using RestSharp;
using Starter.Bootstrapper;
using Starter.Configuration.Entities;
using Starter.Data.Entities;
using Starter.Framework.Clients;
using Starter.Framework.Extensions;

namespace Starter.Framework.Tests.Clients
{
    /// <summary>
    /// Tests for the ApiClient class
    /// </summary>
    [TestFixture]
    public class ApiClientTests: TestsBase
    {
        [Test]
        public void ApiClient_NewInstance_Successful()
        {
        }

        [Test]
        public async Task GetAll_Successful()
        {
            var cats = await ApiClient.GetAll<Cat>();

            cats.Count().Should().Be(Cats.Count);
        }

        [Test]
        public async Task Create_Successful()
        {
            var mockRestClient = new Mock<IRestClient>();
            var apiSettings = IocWrapper.Instance.GetService<IApiSettings>();

            mockRestClient.Setup(x => x.ExecuteTaskAsync(It.Is<RestRequest>((r) => r.Method == Method.POST), It.IsAny<CancellationToken>())).Returns((RestRequest r, CancellationToken token) =>
            {
                var response = new Mock<IRestResponse>();
                response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);

                Cats.Add(r.Parameters[0].Value.ToString().FromJson<Cat>());

                return Task.FromResult(response.Object);
            });

            var cat = new Cat { Id = Guid.NewGuid(), Name = Guid.NewGuid().ToString() };
            var apiClient = new ApiClient(mockRestClient.Object, apiSettings);
            await apiClient.Create(cat);

            Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeEquivalentTo(cat);
        }

        [Test]
        public async Task Update_Successful()
        {
            var mockRestClient = new Mock<IRestClient>();
            var apiSettings = IocWrapper.Instance.GetService<IApiSettings>();

            mockRestClient.Setup(x => x.ExecuteTaskAsync(It.Is<RestRequest>((r) => r.Method == Method.PUT), It.IsAny<CancellationToken>())).Returns((RestRequest r, CancellationToken token) =>
            {
                var response = new Mock<IRestResponse>();
                response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);

                var c = r.Parameters[0].Value.ToString().FromJson<Cat>();
                var existing = Cats.Find(x => x.Id == c.Id);

                Cats.Remove(existing);
                Cats.Add(c);

                return Task.FromResult(response.Object);
            });

            var cat = Cats.FirstOrDefault();
            var newName = Guid.NewGuid().ToString();

            cat.Name = newName;

            var apiClient = new ApiClient(mockRestClient.Object, apiSettings);
            await apiClient.Update(cat);

            Cats.FirstOrDefault(x => x.Name == newName).Should().NotBeNull();
        }

        [Test]
        public async Task Delete_Successful()
        {
            var mockRestClient = new Mock<IRestClient>();
            var apiSettings = IocWrapper.Instance.GetService<IApiSettings>();

            mockRestClient.Setup(x => x.ExecuteTaskAsync(It.Is<RestRequest>((r) => r.Method == Method.DELETE), It.IsAny<CancellationToken>())).Returns((RestRequest r, CancellationToken token) =>
            {
                var response = new Mock<IRestResponse>();
                response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);

                var id = Guid.Parse(r.Parameters[0].Value.ToString());
                Cats.Remove(Cats.FirstOrDefault(x => x.Id == id));

                return Task.FromResult(response.Object);
            });

            var cat = Cats.FirstOrDefault();

            var apiClient = new ApiClient(mockRestClient.Object, apiSettings);
            await apiClient.Delete(cat.Id);

            Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeNull();
        }
    }
}