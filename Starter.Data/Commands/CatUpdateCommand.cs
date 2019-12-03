using Starter.Framework.Clients;

namespace Starter.Data.Commands
{
    public class CatUpdateCommand : CatCommand
    {
        public CatUpdateCommand(IApiClient apiClient) : base(apiClient)
        {
        }
    }
}