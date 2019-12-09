using Microsoft.Extensions.Hosting;

using Starter.Data.Entities;

namespace Starter.Data.Consumers
{
    public interface IMessageConsumerService: IHostedService
    {
        void OnDataReceived(object sender, Message<Cat> message);

        //Task ExecuteAsync(CancellationToken stoppingToken);

        //Task StopAsync(CancellationToken cancellationToken);
    }
}