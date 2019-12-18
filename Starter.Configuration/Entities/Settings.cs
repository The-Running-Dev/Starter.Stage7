namespace Starter.Configuration.Entities
{
    /// <summary>
    /// Implements application settings
    /// </summary>
    public class Settings : ISettings
    {
        public string ApiUrl { get; set; }

        public string ApplicationInsightsInstrumentationKey { get; set; }

        public string CatEntityTableName { get; set; }

        public string StorageAccountConnection { get; set; }

        public string ResourceUrl { get; set; }

        public string ServiceBusConnection { get; set; }

        public string ServiceBusQueue { get; set; }

    }
}