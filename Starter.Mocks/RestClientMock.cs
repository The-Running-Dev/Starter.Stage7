using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

using Moq;
using RestSharp;

using Starter.Data.Entities;
using Starter.Framework.Extensions;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Rest client
    /// </summary>
    public class RestClientMock
    {
        private readonly Mock<IRestClient> _mockRestClient;

        public IRestClient Instance;

        public RestClientMock()
        {
            _mockRestClient = new Mock<IRestClient>();

            Setup<Cat>(Method.GET,
                (request) =>
                {
                    var id = Guid.Parse(request.Parameters[0].Value.ToString());

                    return TestData.Cats.FirstOrDefault(x => x.Id == id);
                });

            Setup<IEnumerable<Cat>>(Method.GET, (request) => TestData.Cats.AsEnumerable());

            Setup(Method.POST,
                (request) =>
                {
                    TestData.Cats.Add(request.Parameters[0].Value.ToString().FromJson<Cat>());
                });

            Setup(Method.PUT,
                (request) =>
                {
                    var c = request.Parameters[0].Value.ToString().FromJson<Cat>();
                    var existing = TestData.Cats.Find(x => x.Id == c.Id);

                    TestData.Cats.Remove(existing);
                    TestData.Cats.Add(c);
                });

            Setup(Method.DELETE,
                (request) =>
                {
                    var id = Guid.Parse(request.Parameters[0].Value.ToString());

                    TestData.Cats.Remove(TestData.Cats.FirstOrDefault(x => x.Id == id));
                });

            Instance = _mockRestClient.Object;
        }

        public void Setup(Method method, Action<RestRequest> setupCallback)
        {
            _mockRestClient.Setup(x =>
                    x.ExecuteTaskAsync(It.Is<RestRequest>((r) => r.Method == method), It.IsAny<CancellationToken>()))
                .Returns((RestRequest request, CancellationToken token) =>
                {
                    var response = new Mock<IRestResponse>();
                    response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);

                    setupCallback(request);

                    return Task.FromResult(response.Object);
                });
        }

        public void Setup<T>(Method method, Func<RestRequest, T> setupCallback) where T : class
        {
            _mockRestClient.Setup(x =>
                    x.ExecuteTaskAsync<T>(It.Is<RestRequest>((r) => r.Method == method), It.IsAny<CancellationToken>()))
                .Returns((RestRequest request, CancellationToken token) =>
                {
                    var response = new Mock<IRestResponse<T>>();
                    response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
                    response.Setup(_ => _.Data).Returns(setupCallback(request));

                    return Task.FromResult(response.Object);
                });
        }

        public void Verify()
        {
            Verify((x) => x.ExecuteTaskAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()), 1);
        }

        public void VerifyNotCalled()
        {
            _mockRestClient.Verify(
                (x) => x.ExecuteTaskAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()),
                Moq.Times.Never);
        }

        public void Verify<T>()
        {
            Verify((x) => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()), 1);
        }

        private void Verify(Expression<Action<IRestClient>> verifyExpression, int times)
        {
            _mockRestClient.Verify(verifyExpression, () => Moq.Times.AtLeast(times));
        }
    }
}