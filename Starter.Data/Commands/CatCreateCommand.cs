using Starter.Framework.Clients;

namespace Starter.Data.Commands
{
    public class CatCreateCommand : CatCommand
    {
        public CatCreateCommand(IApiClient apiClient) : base(apiClient)
        {
        }
    }
}