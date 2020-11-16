using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace LevelEditor
{
    public class NewLevelSignal : Signal
    {
    }
    public class NewLevelCommand : Command
    {
        [Inject] public IEditorLevelsInfoModel LevelsInfoModel { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ShowLevelInfoDataSignal ShowLevelInfoDataSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }


        public override void Execute()
        {
            var levelInfo = LevelsInfoModel.AddEmptyLevelInfo();
            LevelsInfoModel.SetSelectedLevelInfo(levelInfo.Index);
            EnemyEventsModel.ClearAllData();
            
            LevelEditorControler.EventsPerBeat = levelInfo.EventsPerBeat;
            LevelEditorControler.Bmp = levelInfo.BeatsPerMinute;
            
            ShowLevelInfoDataSignal.Dispatch(true);
            LevelEditorControler.IndexChangedSignal.Dispatch();

        }
    }
}