using Framewerk.UI.List;

namespace LevelEditor
{
    public class DirectoriesListItemMediator : ListItemMediator<DirectoriesListItemView, DirectoriesBundleDataprovider>
    {
        public override void SetData(DirectoriesBundleDataprovider dataProvider, int index)
        {
            base.SetData(dataProvider, index);
            View.DirectoryName.text = dataProvider.DirecoryName;
        }
    }
}