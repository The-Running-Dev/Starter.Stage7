namespace Starter.Configuration.Entities
{
    public class ApiSettings: IApiSettings
    {
        public string Url { get; set; }

        public string Resource { get; set; }
    }
}
