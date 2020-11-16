using System.Linq;
using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEditor;

namespace LevelEditor
{
    public class SaveAsSignal : Signal
    {
    }

    public class SaveAsCommand : Command
    {
        [Inject] public IEditorLevelsInfoModel LevelsInfoModel { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelImporter LevelImporter { get; set; }

        [Inject] public IUserConfirmationService UserConfirmationService { get; set; }


        private LevelInfoData _levelInfo;

        public override void Execute()
        {
            Retain();
            // Level info mode, Event data model, Level Importer
            _levelInfo = LevelsInfoModel.GetSelectedLevelInfo();
            UserConfirmationService.GetUserOverwrite(_levelInfo.LevelName).Then(OnUserAccepted);
            // get current map data
        }

        private void OnUserAccepted(bool accepted)
        {
            if (accepted)
            {
                LevelImporter.SaveLevelInfo(_levelInfo);
                LevelImporter.SaveStandartLevel(EnemyEventsModel.AllEnemies, EnemyEventsModel.AllGrass,
                    EnemyEventsModel.AllFish, EnemyEventsModel.AllObstacles, EnemyEventsModel.AllTargets, _levelInfo);
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            Release();
        }
    }
}