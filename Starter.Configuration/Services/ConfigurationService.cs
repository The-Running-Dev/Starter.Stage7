using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace Starter.Configuration.Services
{
    /// <summary>
    /// Implements the Configuration service
    /// </summary>
    public class ConfigurationService: IConfigurationService
    {
        public IConfigurationRoot Configuration { get; set; }

        public ConfigurationService(string env)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public T Get<T>(string sectionName) where T: new()
        {
            var instance = new T();

            GetSection(sectionName).Bind(instance);

            return instance;
        }

        public IConfiguration GetSection(string sectionName)
        {
            return Configuration.GetSection(sectionName);
        }
    }
}