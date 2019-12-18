using System;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using Starter.Data.Entities;
using Starter.Data.Services;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Cat service
    /// </summary>
    public class CatServiceMock
    {
        public ICatService Instance;

        public CatServiceMock()
        {
            var mockCatService = new Mock<ICatService>();

            // Setup the cat service
            mockCatService.Setup(x => x.GetAll()).Returns(Task.FromResult(TestData.Cats.AsEnumerable())).Verifiable();

            mockCatService.Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(TestData.Cats.FirstOrDefault(x => x.Id == id)); })
                .Verifiable();

            mockCatService.Setup(x => x.Create(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    TestData.Cats.Add(entity);

                    return Task.CompletedTask;
                }).Verifiable();

            mockCatService.Setup(x => x.Update(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    var existing = TestData.Cats.Find(x => x.Id == entity.Id);

                    TestData.Cats.Remove(existing);
                    TestData.Cats.Add(entity);

                    return Task.CompletedTask;
                }).Verifiable();

            mockCatService.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                {
                    TestData.Cats.Remove(TestData.Cats.FirstOrDefault(x => x.Id == id));

                    return Task.CompletedTask;
                }).Verifiable();

            Instance = mockCatService.Object;
        }
    }
}