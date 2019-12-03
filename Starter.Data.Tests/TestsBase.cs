using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;

using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Data.ViewModels;
using Starter.Framework.Clients;

namespace Starter.Data.Tests
{
    /// <summary>
    /// Base class for the Starter.Data.Tests project
    /// </summary>
    public class TestsBase
    {
        protected List<Cat> Cats { get; set; }

        protected MainViewModel ViewModel { get; set; }

        protected IApiClient ApiClient { get; set; }

        protected IMessageBroker<Cat> MessageBroker { get; set; }

        protected ICatService CatService { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            CreateCatTestData();

            var mockApiClient = new Mock<IApiClient>();
            var mockServiceBus = new Mock<IMessageBroker<Cat>>();
            var mockCatService = new Mock<ICatService>();

            // Setup the cat service
            mockCatService.Setup(x => x.GetAll()).Returns(Task.FromResult(Cats.AsEnumerable()));

            mockCatService.Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(Cats.FirstOrDefault(x => x.Id == id)); });

            mockCatService.Setup(x => x.Create(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    Cats.Add(entity);

                    return Task.CompletedTask;
                });

            mockCatService.Setup(x => x.Update(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    var existing = Cats.Find(x => x.Id == entity.Id);

                    Cats.Remove(existing);
                    Cats.Add(entity);

                    return Task.CompletedTask;
                });

            mockCatService.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                {
                    Cats.Remove(Cats.FirstOrDefault(x => x.Id == id));
                    return Task.CompletedTask;
                });

            ApiClient = mockApiClient.Object;
            MessageBroker = mockServiceBus.Object;
            CatService = mockCatService.Object;

            ViewModel = new MainViewModel(CatService);
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
}