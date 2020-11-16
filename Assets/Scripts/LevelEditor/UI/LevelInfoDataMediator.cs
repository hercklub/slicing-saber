using System;
using Framewerk.UI;
using TMPro;

namespace LevelEditor
{
    public class LevelInfoDataMediator : ExtendedMediator
    {
        [Inject] public LevelInfoDataView View { get; set; }
        [Inject] public ShowLevelInfoDataSignal ShowLevelInfoDataSignal { get; set; }
        [Inject] public IEditorLevelsInfoModel LevelsInfoModel { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }


        private LevelInfoData _data;
        public override void OnRegister()
        {
            base.OnRegister();
            AddTmpInputListener(View.LevelNameInput, LevelNameHandler);
            AddTmpInputListener(View.DisplayNameInput, DisplayNameHandler);
            AddTmpInputListener(View.BpmInput, BpmHandler);
            AddTmpInputListener(View.EventsPerMinuteInput, EventsPerMinuteHandler);
            AddTmpInputListener(View.DurationInput, DurationeHandler);
            
            AddTmpDropdownListener(View.DiffcultyDropdown, DifficultyHandler);
            AddTmpDropdownListener(View.LevelTypeDropdown, LevelTypeHandler);
            
            ShowLevelInfoDataHandler( false);
            ShowLevelInfoDataSignal.AddListener(ShowLevelInfoDataHandler);
            
            
            var enumValue = Enum.GetValues(typeof(LevelDificulty));
            View.DiffcultyDropdown.options.Clear();
            foreach (var val in enumValue)
            {
                View.DiffcultyDropdown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }
            
            var levelType = Enum.GetValues(typeof(LevelType));
            View.LevelTypeDropdown.options.Clear();
            foreach (var val in levelType)
            {
                View.LevelTypeDropdown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }
        }


        private void LevelNameHandler(string levelName)
        {
            //set level info data
            _data.LevelName = levelName;
        }
        
        private void DisplayNameHandler(string displayName)
        {
            //set level info data
            _data.DisplayLevelName = displayName;
        }
        private void BpmHandler(string bpmInput)
        {
            float bpm;
            if (float.TryParse(bpmInput, out bpm))
            {
                _data.BeatsPerMinute = bpm;
                LevelEditorControler.Bmp = bpm;
                LevelEditorControler.IndexChangedSignal.Dispatch();
                //EnviromentMovedSignal.Dispatch();

            }
        }
        
        private void EventsPerMinuteHandler(string epbInput)
        {
            int epb; // events per beat
            if (int.TryParse(epbInput, out epb))
            {
                _data.EventsPerBeat = epb;
                LevelEditorControler.EventsPerBeat = epb;
                LevelEditorControler.IndexChangedSignal.Dispatch();
                //EnviromentMovedSignal.Dispatch();

            }
        }
        
        private void DurationeHandler(string durtaionInput)
        {
            float duration; // events per beat
            if (float.TryParse(durtaionInput, out duration))
            {
                _data.Duration = duration;
                LevelEditorControler.IndexChangedSignal.Dispatch();
                //EnviromentMovedSignal.Dispatch();

            }
        }
        
        private void DifficultyHandler(int difficulty)
        {
            _data.Difficulty = (LevelDificulty)difficulty;
        }
        
        private void LevelTypeHandler(int levelType)
        {
            _data.LevelType = (LevelType)levelType;
        }

        private void ShowLevelInfoDataHandler(bool isEnabled)
        {
            var levelInfo = LevelsInfoModel.GetSelectedLevelInfo();
            _data = levelInfo;
            if (_data != null)
            {
                Show(_data);
            }
            // show if there is level info created
            View.gameObject.SetActive(isEnabled && levelInfo != null);
        }

        public void Show(LevelInfoData saveData)
        {
            // set data
            _data = saveData;
            SetData(saveData);

        }
        
        private void SetData(LevelInfoData data)
        {
            View.LevelNameInput.text = data.LevelName.ToString();
            if (data.DisplayLevelName != null)
            {
                View.DisplayNameInput.text = data.DisplayLevelName.ToString();
            }
            else
            {
                View.DisplayNameInput.text = string.Empty;
            }

            View.BpmInput.text = data.BeatsPerMinute.ToString();
            View.EventsPerMinuteInput.text = data.EventsPerBeat.ToString();
            View.DurationInput.text = data.Duration.ToString();
            
            View.DiffcultyDropdown.value = (int)data.Difficulty;
            View.LevelTypeDropdown.value = (int)data.LevelType;

        }
    }
}