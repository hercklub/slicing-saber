using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Level;
using LevelEditor.EventsList;
using UnityEngine;

namespace LevelEditor.IO
{
    public interface ILevelImporter
    {
        //level editor
        MapSaveData GetCurrentLevelData(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData, LevelInfoData levelInfo);


        //standard levels
        void SaveLevelInfo(LevelInfoData levelInfo);

        void SaveStandartLevel(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData, LevelInfoData levelInfo);

        EventsData LoadEditorStandardLevel(LevelInfoData levelInfo);
        MapSaveData LoadGameStandardLevel(string levelName);

        List<LevelInfoData> LoadAllLevelInfos();


        // bundles
        void SaveBundleInfo(EditorBundleInfoData bundleInfo);

        void SaveLibraryBundle(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetDat, EditorBundleInfoData bundleInfo, LevelInfoData infoData);

        void LoadLibraryBundle(EditorBundleInfoData bundleInfo, LevelInfoData levelInfo,
            out List<EditorEnemyData> editorEnemyData,
            out List<EditorGrassData> grassEventSaveData,
            out List<EditorFishData> fishEventSaveData,
            out List<EditorObstacleData> obstacleData,
            out List<EditorTargetData> targetData);

        List<EditorBundleInfoData> LoadAllBundleInfos();
        
        void DeleteBundle(EditorBundleInfoData data);
    }

    public class LevelImporter : ILevelImporter
    {
        public MapSaveData GetCurrentLevelData(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData, LevelInfoData levelInfo)
        {
            var enemyEventsSaveData = GetEnemySaveData(enemyData);
            var grassEventSaveData = GetGrassSaveData(grassData);
            var fishEventSaveData = GetFishSaveData(fishData);
            var obstacleEventSaveData = GetObstacleSaveData(obstacleData);
            var targetEventsSaveData = GetTargetSaveData(targetData);
            MapSaveData saveData = new MapSaveData(enemyEventsSaveData, grassEventSaveData, fishEventSaveData, obstacleEventSaveData, targetEventsSaveData);
            return saveData;
        }


        public void SaveLevelInfo(LevelInfoData levelInfo)
        {
            LevelInfoSaveData saveData = new LevelInfoSaveData(levelInfo.LevelName, levelInfo.DisplayLevelName, levelInfo.BeatsPerMinute,
                levelInfo.EventsPerBeat, levelInfo.Duration, levelInfo.Difficulty, levelInfo.LevelType);
            FileHelpers.SaveToJSONFile(saveData, DataPathHelper.LevelInfoPath + levelInfo.LevelFilename,
                Application.persistentDataPath + "/test.temp", Application.persistentDataPath + "/test.bak");
        }

        public void SaveStandartLevel(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData, LevelInfoData levelInfo)
        {
            var enemyEventsSaveData = GetEnemySaveData(enemyData);
            var grassEventSaveData = GetGrassSaveData(grassData);
            var fishEventSaveData = GetFishSaveData(fishData);
            var obstacleEventSaveData = GetObstacleSaveData(obstacleData);
            var targetEventsSaveData = GetTargetSaveData(targetData);
            MapSaveData saveData = new MapSaveData(enemyEventsSaveData, grassEventSaveData, fishEventSaveData, obstacleEventSaveData, targetEventsSaveData);
            
            FileHelpers.SaveToJSONFile(saveData, DataPathHelper.LevelDataPath + levelInfo.LevelFilename,
                Application.persistentDataPath + "/test.temp", Application.persistentDataPath + "/test.bak");
        }

        public EventsData LoadEditorStandardLevel(LevelInfoData levelInfo)
        {
            var levefile =
                Resources.Load(ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_DATA + levelInfo.LevelName) as TextAsset;
            MapSaveData mapSaveData = MapSaveData.DeserializeFromJSONString(levefile.text);

            CheckVersion(mapSaveData, levelInfo);
            //Get all layers
            List<EventLayerData> layerData = new List<EventLayerData>();
            // needs to define STEP,  variable or static

            for (int i = 0; i < 5000; i++)
            {
                layerData.Add(new EventLayerData(i));
            }

            List<EditorEnemyData> editorEnemyData = GetEditorEnemyEvents(ref mapSaveData);
            List<EditorGrassData> grassEventSaveData = GetEditorGrassEvents(ref mapSaveData);
            List<EditorFishData> fishEventSaveData = GetEditorFishEvents(ref mapSaveData);
            List<EditorObstacleData> editorObstacleData = GetEditorObstacleEvents(ref mapSaveData);
            List<EditorTargetData> editorTargetData = GetEditorTargetData(ref mapSaveData);
            EventsData enemyData = new EventsData(layerData, editorEnemyData, grassEventSaveData, fishEventSaveData, editorObstacleData, editorTargetData);
            return enemyData;
        }

        private void CheckVersion(MapSaveData mapSaveData, LevelInfoData levelInfoData)
        {
            if (mapSaveData.version == "0.0.3")
            {
                Debug.LogWarning("[UPGRADE] Old version" + mapSaveData.version);
                float step = 60f / (levelInfoData.BeatsPerMinute * levelInfoData.EventsPerBeat);
                foreach (var enemy in mapSaveData.enemyEvents)
                {
                    enemy.time = enemy.time / step;
                }
                foreach (var fish in mapSaveData.fishEvents)
                {
                    fish.time = fish.time / step;
                }
                foreach (var grass in mapSaveData.grassEvents)
                {
                    grass.time = grass.time / step;
                }
                foreach (var obstacle in mapSaveData.obstacleEvents)
                {
                    obstacle.time = obstacle.time / step;
                }
            }
        }

        public MapSaveData LoadGameStandardLevel(string levelName)
        {
            var levefile = Resources.Load(ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_DATA + levelName) as TextAsset;
            MapSaveData mapSaveData = MapSaveData.DeserializeFromJSONString(levefile.text);
            return mapSaveData;
        }
        


        public List<LevelInfoData> LoadAllLevelInfos()
        {

            List<LevelInfoData> levelInfoDatas = new List<LevelInfoData>();
            //var levelInfo = Resources.Load( ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_INFO + "test1") as TextAsset;
            
            var levelInfos = Resources.LoadAll<TextAsset>(ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_INFO);
            int i = 0;
            foreach (var level in levelInfos)
            {
                var data = LevelInfoSaveData.DeserializeFromJSONString(level.text);
                levelInfoDatas.Add(new LevelInfoData(i, data.Version, data.LevelName, data.DisplayLevelName, data.BeatsPerMinute, 
                    data.EventsPerBeat, data.Duration, data.Difficulty, data.LevelType));
                i++;
            }

            return levelInfoDatas;
        }

        // bundles

        public void SaveBundleInfo(EditorBundleInfoData bundleInfo)
        {
            EditorBundleSaveData saveData = new EditorBundleSaveData(bundleInfo);
            FileHelpers.SaveToJSONFile(saveData, DataPathHelper.BundleInfoPath + bundleInfo.BudnleFilename,
                Application.persistentDataPath + "/test.temp", Application.persistentDataPath + "/test.bak");
        }

        public void SaveLibraryBundle(List<EditorEnemyData> enemyData, List<EditorGrassData> grassData,
            List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData,
            EditorBundleInfoData levelInfo, LevelInfoData infoData)
        {
            var enemyEventsSaveData = GetEnemySaveData(enemyData);
            var grassEventSaveData = GetGrassSaveData(grassData);
            var fishEventSaveData = GetFishSaveData(fishData);
            var obstacleEventSaveData = GetObstacleSaveData(obstacleData);
            var targetEventSaveData = GetTargetSaveData(targetData);
            MapSaveData saveData = new MapSaveData(enemyEventsSaveData, grassEventSaveData, fishEventSaveData, obstacleEventSaveData, targetEventSaveData);

            FileHelpers.SaveToJSONFile(saveData, DataPathHelper.BundleDataPath + levelInfo.BudnleFilename,
                Application.persistentDataPath + "/test.temp", Application.persistentDataPath + "/test.bak");
        }

        public void LoadLibraryBundle(EditorBundleInfoData bundleInfo, LevelInfoData levelInfo,
            out List<EditorEnemyData> editorEnemyData, out List<EditorGrassData> grassEventSaveData,
            out List<EditorFishData> fishEventSaveData, out List<EditorObstacleData> obstacleSaveData,
            out List<EditorTargetData> targetSaveData)
        {
            var bundleFile =
                Resources.Load(ResourcePath.BUNDLES_ROOT + ResourcePath.BUNDLES_DATA + bundleInfo.Bundlename) as
                    TextAsset;
            MapSaveData mapSaveData = MapSaveData.DeserializeFromJSONString(bundleFile.text);
            
            CheckVersion(mapSaveData, levelInfo);
            
            editorEnemyData = GetEditorEnemyEvents(ref mapSaveData);
            grassEventSaveData = GetEditorGrassEvents(ref mapSaveData);
            fishEventSaveData = GetEditorFishEvents(ref mapSaveData);
            obstacleSaveData = GetEditorObstacleEvents(ref mapSaveData);
            targetSaveData = GetEditorTargetData(ref mapSaveData);
        }

        public List<EditorBundleInfoData> LoadAllBundleInfos()
        {
            List<EditorBundleInfoData> bundleInfoData = new List<EditorBundleInfoData>();
            var bundleInfo = Resources.LoadAll<TextAsset>(ResourcePath.BUNDLES_ROOT + ResourcePath.BUNDLES_INFO);
            int i = 0;
            foreach (var bundle in bundleInfo)
            {
                var data = EditorBundleSaveData.DeserializeFromJSONString(bundle.text);
                bundleInfoData.Add(new EditorBundleInfoData(i, data.Version, data.BundleName,
                    data.BundleDirectoryName, data.EnemyCount, data.GrassCount, data.FishCount, data.ObstacleCount, data.TargetCount));
                i++;
            }

            return bundleInfoData;
        }

        public void DeleteBundle(EditorBundleInfoData data)
        {
            string bundleInfoPath = DataPathHelper.BundleInfoPath + data.BudnleFilename;
            string bundleDataPath = DataPathHelper.BundleDataPath + data.BudnleFilename;
            if (File.Exists(bundleInfoPath))
            {
                File.Delete(bundleInfoPath);
            }
            
            if (File.Exists(bundleDataPath))
            {
                File.Delete(bundleDataPath);
            }

        }

        //HELPERS -------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // SAVE to EDITOR
        private List<EditorEnemyData> GetEditorEnemyEvents(ref MapSaveData mapSaveData)
        {
            List<EditorEnemyData> enemies = new List<EditorEnemyData>();
            int layerId = 0;
            for (int i = 0; i < mapSaveData.enemyEvents.Count; i++)
            {
                var eventData = mapSaveData.enemyEvents[i];
                layerId = Mathf.CeilToInt(eventData.time);
                enemies.Add(new EditorEnemyData(i, layerId, eventData));
            }

            return enemies.Count > 0 ? enemies : null;
        }

        private List<EditorGrassData> GetEditorGrassEvents(ref MapSaveData mapSaveData)
        {
            List<EditorGrassData> grass = new List<EditorGrassData>();
            int layerId = 0;
            for (int i = 0; i < mapSaveData.grassEvents.Count; i++)
            {
                var eventData = mapSaveData.grassEvents[i];
                layerId = Mathf.CeilToInt(eventData.time );
                grass.Add(new EditorGrassData(i, layerId, eventData.time, eventData.pos));
            }

            return grass.Count > 0 ? grass : null;
        }

        private List<EditorFishData> GetEditorFishEvents(ref MapSaveData mapSaveData)
        {
            List<EditorFishData> fishes = new List<EditorFishData>();
            int layerId = 0;
            for (int i = 0; i < mapSaveData.fishEvents.Count; i++)
            {
                var eventData = mapSaveData.fishEvents[i];
                layerId = Mathf.CeilToInt(eventData.time);
                fishes.Add(new EditorFishData(i, layerId, eventData.time, eventData.pos, eventData.fishType));
            }

            return fishes.Count > 0 ? fishes : null;
        }
        
        private List<EditorObstacleData> GetEditorObstacleEvents(ref MapSaveData mapSaveData)
        {
            List<EditorObstacleData> obstacles = new List<EditorObstacleData>();
            int layerId = 0;

            for (int i = 0; i < mapSaveData.obstacleEvents.Count; i++)
            {
                var eventData = mapSaveData.obstacleEvents[i];
                layerId = Mathf.CeilToInt(eventData.time);
                obstacles.Add(new EditorObstacleData(i, layerId, eventData));
            }

            return obstacles.Count > 0 ? obstacles : null;
        }
        
        private List<EditorTargetData> GetEditorTargetData(ref MapSaveData mapSaveData)
        {
            List<EditorTargetData> grass = new List<EditorTargetData>();
            int layerId = 0;
            for (int i = 0; i < mapSaveData.targetEvents.Count; i++)
            {
                var eventData = mapSaveData.targetEvents[i];
                layerId = Mathf.CeilToInt(eventData.time );
                grass.Add(new EditorTargetData(i, layerId, eventData.time, eventData.pos, eventData.depth, eventData.targetType));
            }

            return grass.Count > 0 ? grass : null;
        }

        // EDITOR to SAVE
        private List<GrassSaveData> GetGrassSaveData(List<EditorGrassData> editorGrassData)
        {
            List<GrassSaveData> grassData = new List<GrassSaveData>();
            foreach (var enemy in editorGrassData)
            {
                //index + bmp for time
                grassData.Add(new GrassSaveData(enemy.LayerIndex , enemy.StartPos));
            }

            grassData.Sort((p1, p2) => p1.time.CompareTo(p2.time));
            return grassData;
        }

        private List<FishSaveData> GetFishSaveData(List<EditorFishData> editorFishData)
        {
            List<FishSaveData> fishData = new List<FishSaveData>();
            foreach (var enemy in editorFishData)
            {
                //index + bmp for time
                fishData.Add(new FishSaveData(enemy.LayerIndex , enemy.StartPos, enemy.FishType));
            }

            fishData.Sort((p1, p2) => p1.time.CompareTo(p2.time));
            return fishData;
        }

        private List<EnemySaveData> GetEnemySaveData(List<EditorEnemyData> editorEnemyData)
        {
            List<EnemySaveData> enemyData = new List<EnemySaveData>();
            foreach (var enemy in editorEnemyData)
            {
                //index + bmp for time
                enemyData.Add(new EnemySaveData((enemy.LayerIndex), enemy.StartPos, enemy.StartHeight,
                    enemy.Type, enemy.MidPos, enemy.MidHeight, enemy.MidRotation, enemy.EndRotation,
                    enemy.EndDirection, enemy.CutDir));
            }

            enemyData.Sort((p1, p2) => p1.time.CompareTo(p2.time));
            return enemyData;
        }
        
        private List<ObstacleSaveData> GetObstacleSaveData(List<EditorObstacleData> editorEnemyData)
        {
            List<ObstacleSaveData> obstacleData = new List<ObstacleSaveData>();
            foreach (var obstacle in editorEnemyData)
            {
                //index + bmp for time
                obstacleData.Add(new ObstacleSaveData((obstacle.LayerIndex), obstacle.StartPos, obstacle.StartHeight,
                    obstacle.ScaleX, obstacle.ScaleY, obstacle.MidPos, obstacle.MidHeight, obstacle.EndDirection,obstacle.EndRotation, obstacle.Pivot, obstacle.IsPortal));
            }

            obstacleData.Sort((p1, p2) => p1.time.CompareTo(p2.time));
            return obstacleData;
        }
        
        private List<TargetSaveData> GetTargetSaveData(List<EditorTargetData> editorTargetData)
        {
            List<TargetSaveData> targetData = new List<TargetSaveData>();
            foreach (var target in editorTargetData)
            {
                //index + bmp for time
                targetData.Add(new TargetSaveData(target.Time, target.StartPos,  target.Depth, target.TargetType));
            }

            targetData.Sort((p1, p2) => p1.time.CompareTo(p2.time));
            return targetData;
        }
    }
}