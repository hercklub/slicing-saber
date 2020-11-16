using System.Collections.Generic;
using Framewerk.UI.List;

namespace LevelEditor.EventsList
{
    public class EventLayerDataprovider : IListItemDataProvider
    {
        public int Index;
        public float Radius;
        public float Time;
        public List<EditorEnemyData> EnemiesOnLayer;
        public List<EditorGrassData> GrassOnLayer;
        public List<EditorFishData> FishOnLayer;
        public List<EditorObstacleData> ObstaceOnLayer;
        public List<EditorTargetData> TargetsOnLayer;

        public EventLayerDataprovider(int index, float radius, float time, List<EditorEnemyData> enemiesOnLayer, List<EditorGrassData> grassOnLayer, List<EditorFishData> fishOnLayer, List<EditorObstacleData> obstacleOnLayer, List<EditorTargetData> targetsOnLayer)
        {
            Index = index;
            Radius = radius;
            Time = time;
            EnemiesOnLayer = enemiesOnLayer;
            GrassOnLayer = grassOnLayer;
            FishOnLayer = fishOnLayer;
            ObstaceOnLayer = obstacleOnLayer;
            TargetsOnLayer = targetsOnLayer;
        }
    }



}