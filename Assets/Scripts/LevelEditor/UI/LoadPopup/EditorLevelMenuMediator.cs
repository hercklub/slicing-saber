using System.Collections.Generic;
using Framewerk.UI.List;
using LevelEditor.EventsList;
using LevelEditor.IO;

namespace LevelEditor.UI.LoadPopup
{
    public class EditorLevelMenuMediator : ListMediator<EditorLevelMenuView, EditorLevelItemDataProvider>
    {
        [Inject] public IEditorLevelsInfoModel EditorLevelsInfoModel { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelImporter LevelImporter { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public ShowEditorLevelMenuSignal ShowEditorLevelMenuSignal { get; set; }

        [Inject] public ShowLevelInfoDataSignal ShowLevelInfoDataSignal { get; set; }

        private EditorLevelItemDataProvider _selectedData;
        public override void OnRegister()
        {
            base.OnRegister();
            RefreshData();
            AddButtonListener(View.LoadButton, LoadButtonHandler);
            AddButtonListener(View.CloseButton, CloseButtonHandler);

            ShowEditorLevelMenuSignal.AddListener(ShowEditorLevelMenuHandler);
            
            SetEnabled(false);
            SetLoadButtonEnabled(false);
            
        }
        

        private void LoadButtonHandler()
        {
            // clear previous data ... ask about unsaved changes ?
            EnemyEventsModel.ClearAllData();
            
            //level info
            EditorLevelsInfoModel.SetSelectedLevelInfo(_selectedData.LevelInfoData.Index);
            
            //import level by filename, levelimporter
            var levelData = LevelImporter.LoadEditorStandardLevel(_selectedData.LevelInfoData);


            LevelEditorControler.EventsPerBeat = _selectedData.LevelInfoData.EventsPerBeat;
            LevelEditorControler.Bmp = _selectedData.LevelInfoData.BeatsPerMinute;
            
            EnemyEventsModel.LoadEnemyEventsData(levelData);
            LevelEditorControler.IndexChangedSignal.Dispatch();
            ShowLevelInfoDataSignal.Dispatch(true);

            // add data to events data model
            
            SetEnabled(false);
        }

        private void CloseButtonHandler()
        {
            SetEnabled(false);
        }

        protected override void ListItemSelected(int index, EditorLevelItemDataProvider dataProvider)
        {
            base.ListItemSelected(index, dataProvider);
            View.NameText.text = dataProvider.LevelInfoData.LevelName;
            View.BpmText.text = dataProvider.LevelInfoData.BeatsPerMinute.ToString();
            View.DifficultyText.text = dataProvider.LevelInfoData.Difficulty.ToString();
            SetLoadButtonEnabled(true);
            _selectedData = dataProvider;
        }

        private void RefreshData()
        {
            var levelItems = new List<EditorLevelItemDataProvider>();
            var levelInfos = EditorLevelsInfoModel.GetAllLevelsInfo;
            foreach (var levelInfo in levelInfos)
            {
                levelItems.Add(new EditorLevelItemDataProvider(levelInfo));
            }

            SetData(levelItems);
        }

        private void ShowEditorLevelMenuHandler(bool show)
        {
            SetEnabled(show);
            RefreshData();
        }

        private void SetLoadButtonEnabled(bool isEnabled)
        {
            View.LoadButton.interactable = isEnabled;
        }

        private void SetEnabled(bool isEnabled)
        {
            View.gameObject.SetActive(isEnabled);
        }
    }
}