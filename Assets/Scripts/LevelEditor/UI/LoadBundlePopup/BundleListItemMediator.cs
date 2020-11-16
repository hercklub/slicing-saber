using Framewerk.UI.List;

namespace LevelEditor
{
    public class BundleListItemMediator : ListItemMediator<BundleListItemView, BundleDataItemDataProvider>
    {
        public override void SetData(BundleDataItemDataProvider dataProvider, int index)
        {
            base.SetData(dataProvider, index);
            View.BundleName.text = dataProvider.BundleInfoData.Bundlename;
        }
    }
}