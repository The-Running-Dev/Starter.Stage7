using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Moq;

using Starter.Data.Entities;
using Starter.Framework.Clients;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked ApiClient
    /// </summary>
    public class ApiClientMock
    {
        private readonly Mock<IApiClient> _mockApiClient;

        public IApiClient Instance;

        public ApiClientMock()
        {
            _mockApiClient = new Mock<IApiClient>();

            _mockApiClient.Setup(x => x.GetAll<Cat>())
                .Returns(Task.FromResult<IEnumerable<Cat>>(TestData.Cats))
                .Verifiable();

            _mockApiClient.Setup(x => x.GetById<Cat>(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(TestData.Cats.FirstOrDefault(c => c.Id == id)))
                .Verifiable();

            _mockApiClient.Setup(x => x.Create<Cat>(It.IsAny<Cat>()))
                .Returns((Cat cat) =>
                {
                    TestData.Cats.Add(cat);

                    return Task.CompletedTask;
                })
                .Verifiable();

            _mockApiClient.Setup(x => x.Update<Cat>(It.IsAny<Cat>()))
                .Returns((Cat cat) =>
                {
                    var existing = TestData.Cats.Find(c => c.Id == cat.Id);

                    TestData.Cats.Remove(existing);
                    TestData.Cats.Add(cat);

                    return Task.CompletedTask;
                })
                .Verifiable();

            _mockApiClient.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns((Guid id) =>
                {
                    TestData.Cats.Remove(TestData.Cats.FirstOrDefault(x => x.Id == id));

                    return Task.CompletedTask;
                })
                .Verifiable();

            Instance = _mockApiClient.Object;
        }

        public void VerifyGetAll<T>()
        {
            Verify(x => x.GetAll<T>(), 1);
        }

        public void VerifyGetById<T>()
        {
            Verify(x => x.GetById<T>(It.IsAny<Guid>()), 1);
        }

        public void VerifyCreate()
        {
            Verify(x => x.Create(It.IsAny<Cat>()), 1);
        }

        public void VerifyUpdate()
        {
            Verify(x => x.Update(It.IsAny<Cat>()), 1);
        }

        public void VerifyDelete()
        {
            Verify(x => x.Delete(It.IsAny<Guid>()), 1);
        }

        public void VerifyNotCalled()
        {
            _mockApiClient.Verify(
                (x) => x.Create(It.IsAny<Cat>()), Moq.Times.Never);
        }

        private void Verify(Expression<Action<IApiClient>> verifyExpression, int times)
        {
            _mockApiClient.Verify(verifyExpression, () => Moq.Times.AtLeast(times));
        }
    }
}