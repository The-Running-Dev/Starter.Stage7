using Microsoft.Extensions.Configuration;

namespace Starter.Configuration.Services
{
    /// <summary>
    /// Defines the contract for the configuration service
    /// </summary>
    public interface IConfigurationService
    {
        IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets a typed instance of the settings
        /// specified in the section name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        T Get<T>(string sectionName) where T : new();

        /// <summary>
        /// Gets the settings in the specified section name
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        IConfiguration GetSection(string sectionName);
    }
}