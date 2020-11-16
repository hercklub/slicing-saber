using System.Collections.Generic;
using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEditor;
using UnityEngine;

namespace LevelEditor
{
    public class AddtoLibrarySignal : Signal
    {
    }


    public class AddtoLibraryCommand : Command
    {
        [Inject] public IEditorLevelsInfoModel LevelsInfoModel { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelImporter LevelImporter { get; set; }
        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBudnle { get; set; }

        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        public override void Execute()
        {
            var selectedEvents = LevelEditorControler.SelectedEvents;
            if (selectedEvents.Count == 0)
            {
                Debug.Log("Nothing selected, fuck off!");
            }

            var levelInfo = LevelsInfoModel.GetSelectedLevelInfo();
            var budleInfo = EditorLibraryBudnle.GetSelectedBundleInfo();

            List<EditorEnemyData> enemyData = new List<EditorEnemyData>();
            List<EditorGrassData> grassData = new List<EditorGrassData>();
            List<EditorFishData> fishData = new List<EditorFishData>();
            List<EditorObstacleData> obstacleData = new List<EditorObstacleData>();
            List<EditorTargetData> targetData = new List<EditorTargetData>();

            foreach (var editorEvent in selectedEvents)
            {
                if (editorEvent is EditorEnemyData enemy)
                {
                    enemyData.Add(enemy);
                }
                else if (editorEvent is EditorGrassData grass)
                {
                    grassData.Add(grass);
                }
                else if (editorEvent is EditorFishData fish)
                {
                    fishData.Add(fish);
                }
                else if (editorEvent is EditorObstacleData obstacle)
                {
                    obstacleData.Add(obstacle);
                }
                else if (editorEvent is EditorTargetData target)
                {
                    targetData.Add(target);
                }
            }

            budleInfo.EnemyCount = enemyData.Count;
            budleInfo.GrasCount = grassData.Count;
            budleInfo.FishCount = fishData.Count;
            budleInfo.ObstacleCount = obstacleData.Count;
            budleInfo.TargetCount = targetData.Count;

            // get selected data
            // save "info" data
            LevelImporter.SaveBundleInfo(budleInfo);
            // save actual data
            LevelImporter.SaveLibraryBundle(enemyData, grassData, fishData, obstacleData, targetData, budleInfo, levelInfo);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}