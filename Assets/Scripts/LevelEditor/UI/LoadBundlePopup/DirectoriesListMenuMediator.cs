using System.Collections.Generic;
using Framewerk.UI.List;

namespace LevelEditor
{
    public class DirectoriesListMenuMediator : ListMediator<DirectoriesListMenuView, DirectoriesBundleDataprovider>
    {
        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBundleDataModel { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            RefreshData();
        }

        protected override void ListItemSelected(int index, DirectoriesBundleDataprovider dataProvider)
        {
            base.ListItemSelected(index, dataProvider);
            // dispath event to budnle menu mediator
            EditorLibraryBundleDataModel.SetSelectedDirectory(index);
        }

        private void RefreshData()
        {
            EditorLibraryBundleDataModel.UpdateDirectories();
            var directoryItems = new List<DirectoriesBundleDataprovider>();
            List<string> directories = EditorLibraryBundleDataModel.GetAllDirectories();
            
            foreach (var directory in directories)
            {
                directoryItems.Add(new DirectoriesBundleDataprovider(directory));
            }
            
            SetData(directoryItems);
        }
    }
}