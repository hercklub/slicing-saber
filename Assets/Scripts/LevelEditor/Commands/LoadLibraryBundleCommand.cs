using System.Collections.Generic;
using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace LevelEditor
{
    
    public class LoadLibraryBundleSignal : Signal<BundleDataItemDataProvider>
    {
        
    }
    public class LoadLibraryBundleCommand : Command
    {
        [Inject] public ILevelImporter LevelImporter { get; set; }

        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        
        [Inject] public IEditorLevelsInfoModel EditorLevelsInfoModel { get; set; }

        [Inject] public BundleDataItemDataProvider BundleData { get; set; }


        public override void Execute()
        {
             //get current level info
            var levelInfo = EditorLevelsInfoModel.GetSelectedLevelInfo();
            // load bundle selected bundle data
            List<EditorEnemyData> editorEnemyData;
            List<EditorGrassData> grassEventData;
            List<EditorFishData> fishEventData;
            List<EditorObstacleData> obstacleEventData;
            List<EditorTargetData> targetEventData;
            LevelImporter.LoadLibraryBundle(BundleData.BundleInfoData, levelInfo, out editorEnemyData, out grassEventData,
                out fishEventData, out obstacleEventData, out targetEventData);

            List<IEditorData> allEvents = new List<IEditorData>();
            List<IEditorData> allEventData = new List<IEditorData>();
            if (editorEnemyData != null)
            {
                allEvents.AddRange(editorEnemyData);
            }

            if (grassEventData != null)
            {
                allEvents.AddRange(grassEventData);
            }

            if (fishEventData != null)
            {
                allEvents.AddRange(fishEventData);
            }

            if (obstacleEventData != null)
            {
                allEvents.AddRange(obstacleEventData);
            }
            
            if (targetEventData != null)
            {
                allEvents.AddRange(targetEventData);
            }
            // finding start layer, ugly as fak
            int startLayer = 0;
            if (allEvents[0] != null)
            {
                startLayer = allEvents[0].LayerIndex;
            }

            if (editorEnemyData != null && editorEnemyData.Count > 0)
            {
                startLayer = editorEnemyData[0].LayerIndex;
            }

            if (grassEventData != null && grassEventData.Count > 0)
            {
                startLayer = Mathf.Min(grassEventData[0].LayerIndex, startLayer);
            }

            if (fishEventData != null && fishEventData.Count > 0)
            {
                startLayer = Mathf.Min(fishEventData[0].LayerIndex, startLayer);
            }
            
            if (obstacleEventData != null && obstacleEventData.Count > 0)
            {
                startLayer = Mathf.Min(obstacleEventData[0].LayerIndex, startLayer);
            }
            
            if (targetEventData != null && targetEventData.Count > 0)
            {
                startLayer = Mathf.Min(targetEventData[0].LayerIndex, startLayer);
            }


            // recalculate index to the currently vissible
            foreach (var editorData in allEvents)
            {
                editorData.LayerIndex = (editorData.LayerIndex - startLayer) + LevelEditorControler.CurrentLayerIndex;
            }

           
            
            // add to events model
            if (editorEnemyData != null)
            {
                foreach (var enemyData in editorEnemyData)
                {
                    EnemyEventsModel.AddEnemyEvent(enemyData);
                    allEventData.Add(EnemyEventsModel.GetEnemy(enemyData.Index));
                }
            }

            if (grassEventData != null)
            {
                foreach (var grassData in grassEventData)
                {
                    EnemyEventsModel.AddGrassEvent(grassData);
                    allEventData.Add(EnemyEventsModel.GetGrass(grassData.Index));
                }
            }

            if (fishEventData != null)
            {
                foreach (var fishData in fishEventData)
                {
                    EnemyEventsModel.AddFishEvent(fishData);
                    allEventData.Add(EnemyEventsModel.GetFish(fishData.Index));
                }
            }
            
            if (obstacleEventData != null)
            {
                foreach (var obstacleData in obstacleEventData)
                {
                    EnemyEventsModel.AddObstacleEvent(obstacleData);
                    allEventData.Add(EnemyEventsModel.GetObstacle(obstacleData.Index));
                }
            }
            
            if (targetEventData != null)
            {
                foreach (var targetData in targetEventData)
                {
                    EnemyEventsModel.AddTargetEvent(targetData);
                    allEventData.Add(EnemyEventsModel.GetTarget(targetData.Index));
                }
            }
            
            // set as selected
            LevelEditorControler.UpdateSelected();
        }
    }
}