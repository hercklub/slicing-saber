using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEditor;

namespace LevelEditor
{
    public class DeleteLibraryBundleSignal : Signal<BundleDataItemDataProvider>
    {
        
    }
    public class DeleteLibraryBundleCommand : Command
    {
        [Inject] public BundleDataItemDataProvider BundleData { get; set; }
        [Inject] public ILevelImporter LevelImporter { get; set; }
        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBundleData { get; set; }
        [Inject] public IUserConfirmationService UserConfirmationService { get; set; }
        
        public override void Execute()
        {
            Retain();
            UserConfirmationService.BundleDelete(BundleData.BundleInfoData.Bundlename).Then(OnUserAccept);
        }

        private void OnUserAccept(bool accept)
        {
            if (accept)
            {
                LevelImporter.DeleteBundle(BundleData.BundleInfoData);
                EditorLibraryBundleData.RemoveBundleInfoData(BundleData.BundleInfoData.Index);
                EditorLibraryBundleData.DirectorySelectedSignal.Dispatch();
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            Release();
        }
    }
}