using Microsoft.Extensions.Hosting;

using Starter.Data.Entities;

namespace Starter.Data.Services
{
    /// <summary>
    /// Defines the contract for the message consumer service
    /// </summary>
    public interface IMessageService<T>: IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void OnDataReceived(object sender, Message<T> message);
    }
}