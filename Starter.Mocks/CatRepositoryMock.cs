using System;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using Starter.Data.Entities;
using Starter.Data.Repositories;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Cat repository
    /// </summary>
    public class CatRepositoryMock
    {
        public ICatRepository Instance;

        public CatRepositoryMock()
        {
            var catRepository = new Mock<ICatRepository>();

            catRepository.Setup(x => x.GetAll()).Returns(Task.FromResult(TestData.Cats.AsEnumerable()));

            catRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(TestData.Cats.FirstOrDefault(x => x.Id == id)); });

            catRepository.Setup(x => x.GetBySecondaryId(It.IsAny<Guid>()))
                .Returns((Guid id) => { return Task.FromResult(TestData.Cats.FirstOrDefault(x => x.SecondaryId == id)); });

            catRepository.Setup(x => x.Create(It.IsAny<Cat>()))
                .Returns((Cat entity) =>
                {
                    TestData.Cats.Add(entity);
                    return Task.CompletedTask;
                });

            catRepository.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                {
                    TestData.Cats.Remove(TestData.Cats.FirstOrDefault(x => x.Id == id));
                    return Task.CompletedTask;
                });

            Instance = catRepository.Object;
        }
    }
}