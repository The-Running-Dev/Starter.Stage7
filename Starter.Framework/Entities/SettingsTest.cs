namespace Starter.Framework.Entities
{
    /// <summary>
    /// Implements application settings for testing
    /// </summary>
    public class SettingsTest : SettingsDev
    {
        public override string ApiUrl => "http://localhost:55340/";

        public override string CatEntityTableName => "CatsTests";
    }
}