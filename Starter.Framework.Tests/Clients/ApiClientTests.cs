using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using Starter.Mocks;
using Starter.Data.Entities;

namespace Starter.Framework.Tests.Clients
{
    /// <summary>
    /// Tests for the ApiClient class
    /// </summary>
    [TestFixture]
    public class ApiClientTests : TestsBase
    {
        [Test]
        public void ApiClient_NewInstance_Successful()
        {
            ApiClient.Should().NotBeNull();
        }

        [Test]
        public async Task GetAll_Successful()
        {
            var cats = await ApiClient.GetAll<Cat>();

            cats.Count().Should().Be(TestData.Cats.Count);
            RestClientMock.Verify<IEnumerable<Cat>>();
        }

        [Test]
        public async Task Get_ById_Successful()
        {
            var existingCat = TestData.Cats.FirstOrDefault();
            var cat = await ApiClient.GetById<Cat>(existingCat.Id);

            existingCat.Id.Should().Be(cat.Id);
            RestClientMock.Verify<Cat>();
        }

        [Test]
        public async Task Create_Successful()
        {
            var cat = new Cat { Id = Guid.NewGuid(), Name = Guid.NewGuid().ToString() };

            await ApiClient.Create(cat);

            TestData.Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeEquivalentTo(cat);
            RestClientMock.Verify();
        }

        [Test]
        public async Task Update_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var newName = Guid.NewGuid().ToString();

            cat.Name = newName;

            await ApiClient.Update(cat);

            TestData.Cats.FirstOrDefault(x => x.Name == newName).Should().NotBeNull();
            RestClientMock.Verify();
        }

        [Test]
        public async Task Delete_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();

            await ApiClient.Delete(cat.Id);

            TestData.Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeNull();
            RestClientMock.Verify();
        }
    }
}