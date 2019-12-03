using System.Net.Http;

namespace Starter.Framework.Clients
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}