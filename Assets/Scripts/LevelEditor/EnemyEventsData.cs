using System.Collections.Generic;

namespace LevelEditor
{
    public class EventsData
    {
        public List<EventLayerData> EventLayerData
        {
            get => _eventLayers;
        }

        public Dictionary<int, EditorEnemyData> EnemyData
        {
            get => _enemyData;
        }
        public Dictionary<int, EditorGrassData> GrassData
        {
            get => _grassData;
        }
        public Dictionary<int, EditorFishData> FishData
        {
            get => _fishData;
        }
        public Dictionary<int, EditorTargetData> TargetData
        {
            get => _targetData;
        }
        
        public Dictionary<int, EditorObstacleData> ObstacleData
        {
            get => _obstacleData;
        }

        private List<EventLayerData> _eventLayers;
        private int _currentIndex;
        
        private Dictionary<int, EditorEnemyData> _enemyData;
        private Dictionary<int, EditorGrassData> _grassData;
        private Dictionary<int, EditorFishData> _fishData;
        private Dictionary<int, EditorTargetData> _targetData;
        private Dictionary<int, EditorObstacleData> _obstacleData;
        
        public EventsData(List<EventLayerData> eventLayers, int currentIndex, Dictionary<int, EditorEnemyData> enemyData, Dictionary<int, EditorGrassData> grassData, Dictionary<int, EditorFishData> fishData, Dictionary<int, EditorObstacleData> obstacleData, Dictionary<int, EditorTargetData> targetData)
        {
            _eventLayers = eventLayers;
            _currentIndex = currentIndex;
            _enemyData = enemyData;
            _grassData = grassData;
            _fishData = fishData;
            _obstacleData = obstacleData;
            _targetData = targetData;
        }

        public EventsData(List<EventLayerData> eventLayers, List<EditorEnemyData> enemyData, List<EditorGrassData> grassData, List<EditorFishData> fishData, List<EditorObstacleData> obstacleData, List<EditorTargetData> targetData)
        {
            _eventLayers = eventLayers;
            _enemyData = new Dictionary<int, EditorEnemyData>();
            _grassData = new Dictionary<int, EditorGrassData>();
            _fishData = new Dictionary<int, EditorFishData>();
            _obstacleData = new Dictionary<int, EditorObstacleData>();
            _targetData = new Dictionary<int, EditorTargetData>();

            if (enemyData != null)
            {
                foreach (var enemy in enemyData)
                {
                    _enemyData[enemy.Index] = enemy;
                }
            }

            if (grassData != null)
            {
                foreach (var grass in grassData)
                {
                    _grassData[grass.Index] = grass;
                }
            }

            if (fishData != null)
            {
                foreach (var fish in fishData)
                {
                    _fishData[fish.Index] = fish;
                }
            }
            
            if (obstacleData != null)
            {
                foreach (var obstacle in obstacleData)
                {
                    _obstacleData[obstacle.Index] = obstacle;
                }
            }
            
            if (targetData != null)
            {
                foreach (var target in targetData)
                {
                    _targetData[target.Index] = target;
                }
            }
        }
        public EventsData(EventsData other)
        {
            _eventLayers = new List<EventLayerData>(other._eventLayers);
            _currentIndex = other._currentIndex;
            
            _enemyData = new Dictionary<int, EditorEnemyData>(other._enemyData.Count);
            _grassData = new Dictionary<int, EditorGrassData>(other._grassData.Count);
            _fishData = new Dictionary<int, EditorFishData>(other._fishData.Count);
            _obstacleData = new Dictionary<int, EditorObstacleData>(other._obstacleData.Count);
            _targetData = new Dictionary<int, EditorTargetData>(other._obstacleData.Count);
            
            foreach (KeyValuePair<int, EditorEnemyData> entry in other._enemyData)
            {
                _enemyData.Add(entry.Key, new EditorEnemyData(entry.Value));
            }
            foreach (KeyValuePair<int, EditorGrassData> entry in other._grassData)
            {
                _grassData.Add(entry.Key, new EditorGrassData(entry.Value.Index, entry.Value.LayerIndex, entry.Value.Time, entry.Value.StartPos));
            }
            foreach (KeyValuePair<int, EditorFishData> entry in other._fishData)
            {
                _fishData.Add(entry.Key, new EditorFishData(entry.Value.Index, entry.Value.LayerIndex, entry.Value.Time, entry.Value.StartPos, entry.Value.FishType));
            }
            
            foreach (KeyValuePair<int, EditorObstacleData> entry in other._obstacleData)
            {
                _obstacleData.Add(entry.Key, new EditorObstacleData(entry.Value));
            }

            foreach (KeyValuePair<int, EditorTargetData> entry in other._targetData)
            {
                _targetData.Add(entry.Key, new EditorTargetData(entry.Value.Index, entry.Value.LayerIndex, entry.Value.Time, entry.Value.StartPos,  entry.Value.Depth, entry.Value.TargetType));
            }
        }

        public EventsData Clone()
        {
            return new EventsData(this);
        }


    }
}