using Starter.Framework.Clients;

namespace Starter.Data.Commands
{
    public class CatDeleteCommand : CatCommand
    {
        public CatDeleteCommand(IApiClient apiClient) : base(apiClient)
        {
        }
    }
}