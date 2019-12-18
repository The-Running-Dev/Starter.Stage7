using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using Starter.Mocks;
using Starter.Data.Entities;

namespace Starter.Repository.Tests.Repositories
{
    /// <summary>
    /// Tests for the CatRepository class
    /// </summary>
    public class CatRepositoryTests : TestsBase
    {
        [Test]
        [Category("Integration")]
        public async Task GetAll_Cats_Successful()
        {
            var cats = await CatRepository.GetAll();

            cats.Count().Should().BeGreaterThan(0);
        }

        [Test]
        [Category("Integration")]
        public async Task CatById_ForId_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var existingCat = await CatRepository.GetById(cat.Id);

            existingCat.Name.Should().Be(cat.Name);
        }

        [Test]
        [Category("Integration")]
        public async Task CatBySecondaryId_ForSecondaryId_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var existingCat = await CatRepository.GetBySecondaryId(cat.SecondaryId);

            existingCat.Name.Should().Be(cat.Name);
        }

        [Test]
        [Category("Integration")]
        public async Task Create_Cat_Successful()
        {
            var cat = new Cat(Guid.NewGuid().ToString(), Ability.Napping);

            TestData.Cats.Add(cat);
            await CatRepository.Create(cat);

            var existingCat = await CatRepository.GetById(cat.Id);

            existingCat.Id.Should().Be(cat.Id);
        }

        [Test]
        [Category("Integration")]
        public async Task Update_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            cat.Name = Guid.NewGuid().ToString();

            await CatRepository.Update(cat);

            var existingCat = await CatRepository.GetById(cat.Id);

            existingCat.Id.Should().Be(cat.Id);
        }

        [Test]
        [Category("Integration")]
        public async Task Delete_Cat_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            cat.Name = Guid.NewGuid().ToString();

            await CatRepository.Delete(cat.Id);
            TestData.Cats.Remove(cat);

            var existingCat = await CatRepository.GetById(cat.Id);

            existingCat.Should().BeNull();
        }

        [Test]
        [Category("Integration")]
        public async Task GetAll_Gets3Cats_Successful()
        {
            var results = await CatRepository.GetAll();

            results.Count().Should().BeGreaterThan(0);
        }

        [Test]
        [Category("Integration")]
        public async Task GetAll_SpecificCatExists_Successful()
        {
            var cat = TestData.Cats.FirstOrDefault();
            var cats = await CatRepository.GetAll();

            cats.FirstOrDefault(x => x.Name == cat.Name).Should().NotBeNull();
        }
    }
}