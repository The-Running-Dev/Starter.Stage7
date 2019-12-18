using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using Starter.Mocks;
using Starter.Data.Entities;

namespace Starter.Data.Tests.Services
{
    /// <summary>
    /// Tests for the CatService class
    /// </summary>
    [TestFixture]
    public class CatServiceTests: TestsBase
    {
        [Test]
        public void New_CatServiceInstance_Successful()
        {
            CatService.Should().NotBeNull();
        }

        [Test]
        public async Task GetAll_Cats_Successful()
        {
            var cats = await CatService.GetAll();

            cats.Count().Should().BeGreaterThan(0);
            ApiClientMock.VerifyGetAll<Cat>();
        }

        [Test]
        public async Task GetById_ForCatId_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var existingCat = await CatService.GetById(cat.Id);

            existingCat.Name.Should().Be(cat.Name);
            ApiClientMock.VerifyGetById<Cat>();
        }

        [Test]
        public async Task Create_Cat_Successful()
        {
            var cat = new Cat(Guid.NewGuid().ToString(), Ability.Napping);

            await CatService.Create(cat);

            MessageBrokerMock.Verify(MessageCommand.Create);
        }

        [Test]
        public async Task Update_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            cat.Name = Guid.NewGuid().ToString();

            await CatService.Update(cat);

            MessageBrokerMock.Verify(MessageCommand.Update);
        }

        [Test]
        public async Task Delete_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            cat.Name = Guid.NewGuid().ToString();

            await CatService.Delete(cat.Id);

            MessageBrokerMock.Verify(MessageCommand.Delete);
        }
    }
}