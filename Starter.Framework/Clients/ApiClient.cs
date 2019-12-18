using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;
using Starter.Configuration.Entities;
using Starter.Framework.Extensions;

namespace Starter.Framework.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly IApiSettings _settings;

        private readonly IRestClient _client;

        public ApiClient(IRestClient restClient, IApiSettings settings)
        {
            _client = restClient;
            _settings = settings;

            _client.BaseUrl = new Uri(settings.Url);
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var request = new RestRequest(_settings.Resource, Method.GET);
            var cancellationTokenSource = new CancellationTokenSource();

            var restResponse =
                await _client.ExecuteTaskAsync<IEnumerable<T>>(request, cancellationTokenSource.Token);

            return restResponse.Data ?? new List<T>();
        }

        public async Task<T> GetById<T>(Guid id)
        {
            var request = new RestRequest(_settings.Resource, Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);

            var cancellationTokenSource = new CancellationTokenSource();

            var restResponse =
                await _client.ExecuteTaskAsync<T>(request, cancellationTokenSource.Token);

            return restResponse.Data;
        }

        public async Task Create<T>(T entity)
        {
            await SendEntity(entity, Method.POST);
        }

        public async Task Update<T>(T entity)
        {
            await SendEntity(entity, Method.PUT);
        }

        public async Task Delete(Guid id)
        {
            var request = new RestRequest(_settings.Resource, Method.DELETE);
            var cancellationTokenSource = new CancellationTokenSource();

            request.AddParameter(nameof(id), id);

            await _client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
        }

        private async Task SendEntity<T>(T entity, Method method)
        {
            var request = new RestRequest(_settings.Resource, method);
            var cancellationTokenSource = new CancellationTokenSource();

            request.AddJsonBody(((object)entity).ToJson());

            await _client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
        }
    }
}