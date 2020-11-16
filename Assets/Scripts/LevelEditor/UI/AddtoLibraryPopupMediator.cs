using Framewerk.UI;
using strange.extensions.mediation.impl;

namespace LevelEditor
{
    public class AddtoLibraryPopupMediator : ExtendedMediator
    {
        [Inject] public AddtoLibraryPopupView View { get; set; }
        [Inject] public AddtoLibrarySignal AddtoLibrarySignal { get; set; }

        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBudnle { get; set; }

        private EditorBundleInfoData _bundleInfoData;
        public override void OnRegister()
        {
            //test
            _bundleInfoData = EditorLibraryBudnle.AddEmptyBundleData();
            EditorLibraryBudnle.SetSelectedBundleInfo(_bundleInfoData.Index);
            
            AddTmpInputListener(View.BundleName, BundleNameHandler);
            AddTmpInputListener(View.DirectoryName, DirectoryNameHandler);
            AddButtonListener(View.SaveButton, SaveButtonHandler);
            AddButtonListener(View.CloseButton, CloseButtonHandler);

            base.OnRegister();
        }

        private void BundleNameHandler(string bundleName)
        {
            _bundleInfoData.Bundlename = bundleName;
        }
        
        private void DirectoryNameHandler(string direcotyName)
        {
            _bundleInfoData.BundleDirectory = direcotyName;
        }

        private void SaveButtonHandler()
        {
            AddtoLibrarySignal.Dispatch();
            CloseButtonHandler();
        }
        
        private void CloseButtonHandler()
        {
            Destroy(this.gameObject);
        }
        public override void OnRemove()
        {
            base.OnRemove();
        }
    }
}