using Framewerk.UI.List;

namespace LevelEditor
{
    public class BundleDataItemDataProvider : IListItemDataProvider
    {
        public EditorBundleInfoData BundleInfoData;
        public BundleDataItemDataProvider(EditorBundleInfoData bundleInfoData)
        {
            BundleInfoData = bundleInfoData;
        }
    }
}