namespace Starter.Configuration.Entities
{
    public interface IApiSettings
    {
        string Url { get; set; }

        string Resource { get; set; }
    }
}