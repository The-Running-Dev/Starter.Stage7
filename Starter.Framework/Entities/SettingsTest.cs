namespace Starter.Framework.Entities
{
    /// <summary>
    /// Implements application settings for testing
    /// </summary>
    public class SettingsTest : SettingsDev
    {
        public override string CatEntityTableName => "CatsTests";

        public override string StorageAccountConnection =>
            "DefaultEndpointsProtocol=https;AccountName=devastraboyankostadinov;AccountKey=Tt+oWyP1oBSYOgFGLqonhOVEtAVp+q2GaCMe32IZR0AY9fL87PjofMnILJpyUMRJM8+LTgyM/FQ82LyUahWd1Q==;EndpointSuffix=core.windows.net";
    }
}