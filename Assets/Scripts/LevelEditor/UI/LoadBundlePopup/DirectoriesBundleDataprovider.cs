using Framewerk.UI.List;

namespace LevelEditor
{
    public class DirectoriesBundleDataprovider : IListItemDataProvider
    {
        public string DirecoryName { get; }

        public DirectoriesBundleDataprovider(string direcoryName)
        {
            DirecoryName = direcoryName;
        }
    }
}