namespace Starter.Framework.Entities
{
    /// <summary>
    /// Implements base application settings
    /// </summary>
    public class Settings : ISettings
    {
        public virtual string ApiUrl => "https://boyankostadinov-api.azurewebsites.net/";

        public virtual string ApplicationInsightsInstrumentationKey => "0892a77e-7112-46a3-a207-938992438722";

        public virtual string CatEntityTableName => "Cats";

        public virtual string TableStorageConnectionString =>
            "DefaultEndpointsProtocol=https;AccountName=boyankostadinovstorage;AccountKey=oiZV7i0kSnrOfg2Vpg6Xlr+OxfJ6tIMChvvaptIxopO+pPtS2Yf/7csE8goxIgzSHqLRKXt6QVuvaUUX/yyaBw==;EndpointSuffix=core.windows.net";
        
        public virtual string ResourceUrl => "/cat";
        
        public virtual string ServiceBusConnectionString => "Endpoint=sb://boyankostadinov.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1uTONliIWAhDgJpq55UMkmwS9JtMR2R0LFBnwqsLi7g=";

        public virtual string ServiceBusQueue => "starter.queue";

    }
}