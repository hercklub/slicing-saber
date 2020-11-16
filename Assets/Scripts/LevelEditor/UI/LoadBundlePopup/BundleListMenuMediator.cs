using System.Collections.Generic;
using Framewerk.UI;
using Framewerk.UI.List;
using LevelEditor.IO;
using UnityEngine;

namespace LevelEditor
{
    public class BundleListMenuMediator : ListMediator<BundleListMenuView, BundleDataItemDataProvider>
    {
        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBundleDataModel { get; set; }
        
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        [Inject] public LoadLibraryBundleSignal LoadLibraryBundleSignal { get; set; }

        [Inject] public DeleteLibraryBundleSignal DeleteLibraryBundleSignal { get; set; }

        
        public override void OnRegister()
        {
            base.OnRegister();
            RefreshData();
            AddButtonListener(View.LoadButton, LoadButtonHandler);
            AddButtonListener(View.CloseButton, CloseButtonHandler);
            AddButtonListener(View.RemoveButton, RemoveButtonHandler);
            EditorLibraryBundleDataModel.DirectorySelectedSignal.AddListener(DirectorySelectedHandler);

            SetButtonsEnabled(false);
        }

        public override void OnRemove()
        {
            EditorLibraryBundleDataModel.DirectorySelectedSignal.RemoveListener(DirectorySelectedHandler);

            base.OnRemove();
        }


        private void LoadButtonHandler()
        {
            BundleDataItemDataProvider selected = GetSelectedItem();
            if (selected == null)
                return;

            // load data, add to model, recalculate, update
            LoadLibraryBundleSignal.Dispatch(selected);

            // update
            LevelEditorControler.IndexChangedSignal.Dispatch();
            CloseButtonHandler();
        }

        private void RemoveButtonHandler()
        {
            BundleDataItemDataProvider selected = GetSelectedItem();
            
            if (selected == null)
                return;
            
            DeleteLibraryBundleSignal.Dispatch(selected);
        }

        protected override void ListItemSelected(int index, BundleDataItemDataProvider dataProvider)
        {
            base.ListItemSelected(index, dataProvider);
            SetButtonsEnabled(true);
            
            BundleDataItemDataProvider selected = GetSelectedItem();
            if (selected != null)
            {
                View.StatsContainer.SetActive(true);
                View.BundleNameText.text = selected.BundleInfoData.Bundlename;
                View.EnemyCountText.text = selected.BundleInfoData.EnemyCount.ToString();
                View.GrassCountText.text = selected.BundleInfoData.GrasCount.ToString();
                View.FishCountText.text = selected.BundleInfoData.FishCount.ToString();
            }
            else
            {
                View.StatsContainer.SetActive(false);
            }
        }
        private void DirectorySelectedHandler()
        {
            RefreshData();
        }

        private void CloseButtonHandler()
        {
            Destroy(this.gameObject);
        }

        private void SetButtonsEnabled(bool isEnabled)
        {
            View.LoadButton.interactable = isEnabled;
            View.RemoveButton.interactable = isEnabled;
        }
        
        private void RefreshData()
        {
            var bundleItems = new List<BundleDataItemDataProvider>();
            string selectedDirectory = EditorLibraryBundleDataModel.GetSelectedDirectory();
            
            if (selectedDirectory != null)
            {
                var bundleInfos = EditorLibraryBundleDataModel.GetAllBundleDataByDirectory(selectedDirectory);
                foreach (var bundleInfo in bundleInfos)
                {
                    bundleItems.Add(new BundleDataItemDataProvider(bundleInfo));
                }
            }
            
            View.StatsContainer.SetActive(false);
            SetButtonsEnabled(false);


            SetData(bundleItems);
        }
    }
}