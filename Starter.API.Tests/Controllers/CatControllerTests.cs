using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Starter.Mocks;
using Starter.Data.Entities;

namespace Starter.API.Tests.Controllers
{
    /// <summary>
    /// Tests for the CatController class
    /// </summary>
    [TestFixture]
    public class CatControllerTests : TestsBase
    {
        [Test]
        public async Task GetAll_Cats_Successful()
        {
            var result = await CatController.GetAll() as OkObjectResult;
            var entities = result.Value as IEnumerable<Cat>;

            entities.Count().Should().Be(TestData.Cats.Count);
        }

        [Test]
        public async Task GetCatById_ForId_Successful()
        {
            var lastCat = TestData.Cats.LastOrDefault();
            var result = await CatController.GetById(lastCat.Id) as OkObjectResult;
            var cat = result.Value as Cat;

            cat.Id.Should().Be(lastCat.Id);
        }

        [Test]
        public async Task GetCatBySecondaryId_ForSecondaryId_Successful()
        {
            var lastCat = TestData.Cats.LastOrDefault();
            var result = await CatController.GetBySecondaryId(lastCat.SecondaryId) as OkObjectResult;
            var cat = result.Value as Cat;

            cat.Id.Should().Be(lastCat.Id);
        }

        [Test]
        public async Task Create_Cat_Successful()
        {
            var cat = new Cat() { Id = Guid.NewGuid(), Name = Guid.NewGuid().ToString() };

            await CatController.Post(cat);

            TestData.Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeEquivalentTo(cat);
        }

        [Test]
        public async Task Update_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var newName = Guid.NewGuid().ToString();

            cat.Name = newName;

            await CatController.Put(cat);

            TestData.Cats.FirstOrDefault(x => x.Name == newName).Should().NotBeNull();
        }

        [Test]
        public async Task Delete_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();

            await CatController.Delete(cat.Id);

            TestData.Cats.FirstOrDefault(x => x.Id == cat.Id).Should().BeNull();
        }
    }
}
