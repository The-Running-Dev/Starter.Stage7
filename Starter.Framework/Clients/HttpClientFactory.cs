using System.Net.Http;

namespace Starter.Framework.Clients
{
    public class HttpClientFactory: IHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}