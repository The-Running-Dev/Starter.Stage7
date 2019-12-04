namespace Starter.Framework.Entities
{
    /// <summary>
    /// Implements application settings for development
    /// </summary>
    public class SettingsDev : Settings
    {
        public override string StorageAccountConnection =>
            "UseDevelopmentStorage=true";
    }
}