namespace Starter.Framework.Entities
{
    /// <summary>
    /// Defines the contract for the application settings
    /// </summary>
    public interface ISettings
    {
        string ApiUrl { get; }

        string ApplicationInsightsInstrumentationKey { get; }

        string CatEntityTableName { get; }

        string TableStorageConnectionString { get; }
        
        string ResourceUrl { get; }
        
        string ServiceBusConnectionString { get; }

        string ServiceBusQueue { get; }
    }
}