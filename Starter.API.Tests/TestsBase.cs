using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.API.Controllers;
using Starter.Data.Repositories;

namespace Starter.API.Tests
{
    /// <summary>
    /// Base class for the Starter.API.Tests project
    /// </summary>
    //[SetUpFixture]
    public class TestsBase
    {
        protected List<Cat> Cats { get; set; }

        protected CatController CatController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serviceCollation = new ServiceCollection();
            Setup.Bootstrap(serviceCollation, SetupType.Test);

            CreateCatTestData();

            var catRepository = new Mock<ICatRepository>();

            // Setup the cat repository
            catRepository.Setup(x => x.GetAll()).Returns(Task.FromResult(Cats.AsEnumerable()));

            catRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(Cats.FirstOrDefault(x => x.Id == id)); });

            catRepository.Setup(x => x.GetBySecondaryId(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(Cats.FirstOrDefault(x => x.SecondaryId == id)); });

            catRepository.Setup(x => x.Create(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    Cats.Add(entity);
                    return Task.CompletedTask;
                });

            catRepository.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                {
                    Cats.Remove(Cats.FirstOrDefault(x => x.Id == id));
                    return Task.CompletedTask;
                });

            CatController = new CatController(catRepository.Object);
        }

        protected void CreateCatTestData()
        {
            Cats = new List<Cat>
            {
                new Cat("Widget", Ability.Eating),
                new Cat("Garfield", Ability.Engineering),
                new Cat("Mr. Boots", Ability.Lounging)
            };
        }
    }
}