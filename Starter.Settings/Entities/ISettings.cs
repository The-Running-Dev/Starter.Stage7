namespace Starter.Configuration.Entities
{
    /// <summary>
    /// Defines the contract for the application settings
    /// </summary>
    public interface ISettings
    {
        string ApiUrl { get; set; }

        string ApplicationInsightsInstrumentationKey { get; set; }

        string CatEntityTableName { get; set; }

        string StorageAccountConnection { get; set; }

        string ResourceUrl { get; set; }

        string ServiceBusConnection { get; set; }

        string ServiceBusQueue { get; set; }
    }
}