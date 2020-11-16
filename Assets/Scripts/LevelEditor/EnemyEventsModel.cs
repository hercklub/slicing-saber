using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

namespace LevelEditor
{
    public struct EventLayerData
    {
        public int Index;

        public EventLayerData(int index)
        {
            Index = index;
        }
    }

    public interface IEditorData
    {
        int Index { get; set; }
        int LayerIndex { get; set; }
        float Time { get; set; }
        float StartPos { get; set; }
        float StartHeight { get; set; }
        EnemyType EnemyType { get; }

    }

    public interface IPathEditorData
    {
        float StartPos { get; set; }
        float StartHeight { get; set; }
        float MidPos { get; set; }
        float MidHeight { get; set; }
        EnemyType EnemyType { get; }
    }

    public class EditorEnemyData : IEditorData, IPathEditorData
    {
        //start
        public int Index { get; set; }
        public int LayerIndex { get; set; }
        public float Time { get; set; }
        public float StartPos { get; set; }
        public float StartHeight { get; set; }
        public EnemyAirType Type;

        //mid
        public float MidPos { get; set; }
        public float MidHeight { get; set; }
        public EnemyRotation MidRotation { get; set; }

        //end
        public EnemyDirection EndDirection { get; set; }
        public EnemyRotation EndRotation { get; set; }

        public eCutDir CutDir { get; set; }


        public EnemyType EnemyType => EnemyType.Air;

        public EditorEnemyData(int index, int layerIndex, float time, float posOnLayer, float height,
            EnemyAirType airType, float midPos, float midHeight, EnemyRotation midRotation, EnemyDirection direction,
            EnemyRotation rotation, eCutDir cutDir)
        {
            Index = index;
            LayerIndex = layerIndex;
            Time = time;
            StartPos = posOnLayer;
            StartHeight = height;
            Type = airType;

            MidPos = midPos;
            MidHeight = midHeight;
            MidRotation = midRotation;

            EndDirection = direction;
            EndRotation = rotation;
            CutDir = cutDir;
        }

        public EditorEnemyData(EditorEnemyData data)
        {
            Index = data.Index;
            LayerIndex = data.LayerIndex;
            Time = data.Time;
            StartPos = data.StartPos;
            StartHeight = data.StartHeight;

            Type = data.Type;
            MidPos = data.MidPos;
            MidHeight = data.MidHeight;
            MidRotation = data.MidRotation;

            EndDirection = data.EndDirection;
            EndRotation = data.EndRotation;
            CutDir = data.CutDir;
        }

        public EditorEnemyData(int layerIndex, int time, float posOnLayer, EnemyAirType type, eCutDir cutDir)
        {
            LayerIndex = layerIndex;
            StartPos = posOnLayer;
            MidPos = posOnLayer;
            Time = time;
            Type = type;
            CutDir = cutDir;
        }

        public EditorEnemyData(int index, int layerId, EnemySaveData eventData)
        {
            Index = index;
            LayerIndex = layerId;
            Time = eventData.time;
            StartPos = eventData.spos;
            StartHeight = eventData.sheight;
            Type = eventData.type;
            MidPos = eventData.mpos;
            MidHeight = eventData.mheight;
            MidRotation = eventData.mrot;
            EndDirection = eventData.edir;
            EndRotation = eventData.erot;
            CutDir = eventData.cutdir;
        }
    }

    public class EditorObstacleData : IEditorData, IPathEditorData
    {
        //start
        public int Index { get; set; }
        public int LayerIndex { get; set; }
        public float Time { get; set; }
        public float StartPos { get; set; }
        public float StartHeight { get; set; }

        public float ScaleX { get; set; }

        public float ScaleY { get; set; }

        //mid
        public float MidPos { get; set; }
        public float MidHeight { get; set; }

        //end
        public ObstacleEndDir EndDirection { get; set; }
        public float EndRotation { get; set; }
        
        public ObstacleEndDir Pivot { get; set; }
        public bool IsPortal { get; set; }

        public EnemyType EnemyType => EnemyType.Obstacle;

        public EditorObstacleData(int index, int layerIndex, float time, float startPos, float startHeight,
            float scaleX, float scaleY, float midPos, float midHeight, ObstacleEndDir endDirection, float endRotation, ObstacleEndDir pivot, bool isPortal)
        {
            Index = index;
            LayerIndex = layerIndex;
            Time = time;

            StartPos = startPos;
            StartHeight = startHeight;

            ScaleX = scaleX;
            ScaleY = scaleY;

            MidPos = midPos;
            MidHeight = midHeight;

            EndDirection = endDirection;
            EndRotation = endRotation;

            Pivot = pivot;
            IsPortal = isPortal;

        }

        public EditorObstacleData(EditorObstacleData data)
        {
            Index = data.Index;
            LayerIndex = data.LayerIndex;
            Time = data.Time;

            StartPos = data.StartPos;
            StartHeight = data.StartHeight;

            ScaleX = data.ScaleX;
            ScaleY = data.ScaleY;

            MidPos = data.MidPos;
            MidHeight = data.MidHeight;

            EndDirection = data.EndDirection;
            EndRotation = data.EndRotation;

            Pivot = data.Pivot;
            IsPortal = data.IsPortal;
        }

        public EditorObstacleData(int layerIndex, int time, float posOnLayer, float scale)
        {
            LayerIndex = layerIndex;
            StartPos = posOnLayer;
            MidPos = posOnLayer;
            Time = time;
            ScaleX = scale;
            ScaleY = scale;

            Pivot = ObstacleEndDir.Mid;
            IsPortal = false;
            
        }

        public EditorObstacleData(int index, int layerId, ObstacleSaveData eventData)
        {
            Index = index;
            LayerIndex = layerId;
            Time = eventData.time;

            StartPos = eventData.spos;
            StartHeight = eventData.sheight;

            ScaleX = eventData.scaleX;
            ScaleY = eventData.scaleY;

            MidPos = eventData.mpos;
            MidHeight = eventData.mheight;
            
            EndDirection = eventData.edir;
            EndRotation = eventData.erot;

            Pivot = eventData.pivot;
            IsPortal = eventData.portal;
        }
    }

    public class EditorGrassData : IEditorData
    {
        public int Index { get; set; }
        public int LayerIndex { get; set; }
        public float Time { get; set; }
        public float StartPos { get; set; }
        public float StartHeight { get; set; }
        public EnemyType EnemyType => EnemyType.Grass;


        public EditorGrassData(int index, int layerIndex, float time, float posOnLayer)
        {
            Index = index;
            LayerIndex = layerIndex;
            Time = time;
            StartPos = posOnLayer;
        }
    }
    
    public class EditorTargetData : IEditorData
    {
        public int Index { get; set; }
        public int LayerIndex { get; set; }
        public float Time { get; set; }
        public float StartPos { get; set; }
        public float StartHeight { get; set; }
        
        public float Depth { get; set; }
        public EnemyType EnemyType => EnemyType.Target;
        public TargetType TargetType { get; set; }

        public EditorTargetData(int index, int layerIndex, float time, float posOnLayer, float depth, TargetType target)
        {
            Index = index;
            LayerIndex = layerIndex;
            Time = time;
            StartPos = posOnLayer;
            Depth = depth;
            TargetType = target;
        }
    }

    public class EditorFishData : IEditorData
    {
        public int Index { get; set; }
        public int LayerIndex { get; set; }
        public float Time { get; set; }
        public float StartPos { get; set; }
        public float StartHeight { get; set; }
        public EnemyType EnemyType => EnemyType.Fish;
        public BombType FishType { get; set; }


        public EditorFishData(int index, int layerIndex, float time, float posOnLayer, BombType fishType)
        {
            Index = index;
            LayerIndex = layerIndex;
            Time = time;
            StartPos = posOnLayer;
            FishType = fishType;
        }
    }


    public interface IEnemyEventsModel
    {
        void Init();
        void AddLayer(EventLayerData data);

        EventsData EnemyEventData { get; }

        List<int> VisibleLayerIndexes { get; }

        void LoadEnemyEventsData(EventsData data);

// enemies
        void AddEnemyEvent(int layerIndex, float time, float startPos, float startHeight, EnemyAirType type,
            float midPos, float midHeight, EnemyRotation midRotation,
            EnemyDirection endDirection, EnemyRotation endRotation, eCutDir cutDir);

        void AddEnemyEvent(EditorEnemyData data);

        void RemoveEnemy(int enemyIndex);

        List<EditorEnemyData> AllEnemies { get; }
        List<EditorEnemyData> GetAllEnemisOnLayer(int layerIndex);
        EditorEnemyData GetEnemy(int enemyIndex);


// obstacles
        void AddObstacleEvent(EditorObstacleData data);
        void RemoveObstacle(int enemyIndex);
        List<EditorObstacleData> AllObstacles { get; }
        List<EditorObstacleData> GetAllObstaclesOnLayer(int layerIndex);
        EditorObstacleData GetObstacle(int grassIndex);

// grass
        void AddGrassEvent(int layerIndex, float time, float posOnLayer);
        void AddGrassEvent(EditorGrassData data);
        void RemoveGrass(int grassIndex);
        List<EditorGrassData> AllGrass { get; }
        List<EditorGrassData> GetAllGrasssOnLayer(int layerIndex);
        EditorGrassData GetGrass(int grassIndex);


// fish
        void AddFishEvent(int layerIndex, float time, float posOnLayer, BombType fishType);
        void AddFishEvent(EditorFishData data);
        void RemoveFish(int fishIndex);
        List<EditorFishData> AllFish { get; }
        List<EditorFishData> GetAllFishOnLayer(int layerIndex);
        EditorFishData GetFish(int fishIndex);
        
        
// target
        void AddTargetEvent(int layerIndex, float time, float posOnLayer, float depth, TargetType targetType);
        void AddTargetEvent(EditorTargetData data);
        void RemoveTarget(int targetIndex);
        List<EditorTargetData> AllTargets { get; }
        List<EditorTargetData> GetAllTargetsOnLayer(int layerIndex);
        EditorTargetData GetTarget(int targetIndex);


        IEditorData GetEventData(EnemyType enemyType, int index);
        List<IEditorData> GetAllEventsOnLayer(int layerIndex);
        void RemoveEventData(IEditorData data);

        IEditorData GetLastEventData();
        void ClearAllData();

        void SaveChange();
        void Undo();
        void Redo();
    }

    public class EnemyEventsModel : IEnemyEventsModel
    {
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }


        public EventsData EnemyEventData => _enemyEventData;

        public List<int> VisibleLayerIndexes => _visibleLayerIndexes;

        private List<int> _visibleLayerIndexes = new List<int>();
        private int _enemyIndex = 0;
        private int _grassIndex = 0;
        private int _fishIndex = 0;
        private int _obstaclesIndex = 0;
        private int _targetIndex = 0;

        private EventsData _enemyEventData =
            new EventsData(new List<EventLayerData>(), 0,
                new Dictionary<int, EditorEnemyData>(),
                new Dictionary<int, EditorGrassData>(),
                new Dictionary<int, EditorFishData>(),
                new Dictionary<int, EditorObstacleData>(),
                new Dictionary<int, EditorTargetData>()
            );

        private UndoRedoBuffer<EventsData> _enemyEventsBuffer = new UndoRedoBuffer<EventsData>(40);


        public void Init()
        {
            for (int i = 0; i < 5000; i++)
            {
                AddLayer(new EventLayerData(i));
            }

            LevelEditorControler.IndexChangedSignal.AddListener(IndexChangedHandler);
            IndexChangedHandler();
            SaveChange();
        }


        public void AddLayer(EventLayerData data)
        {
            EnemyEventData.EventLayerData.Add(data);
        }

        // ENEMIES
        public void AddEnemyEvent(int layerIndex, float time, float startPos, float startHeight, EnemyAirType type,
            float midPos, float midHeight, EnemyRotation midRotation,
            EnemyDirection endDirection, EnemyRotation endRotation, eCutDir cutDir)
        {
            EnemyEventData.EnemyData[_enemyIndex] = new EditorEnemyData(_enemyIndex, layerIndex, time, startPos,
                startHeight, type, midPos, midHeight, midRotation, endDirection, endRotation, cutDir);
            _enemyIndex++;
        }

        public void AddEnemyEvent(EditorEnemyData data)
        {
            EditorEnemyData dataCopy = new EditorEnemyData(_enemyIndex, data.LayerIndex, data.Time, data.StartPos,
                data.StartHeight, data.Type, data.MidPos, data.MidHeight, data.MidRotation, data.EndDirection,
                data.EndRotation, data.CutDir);
            EnemyEventData.EnemyData[_enemyIndex] = dataCopy;
            _enemyIndex++;
        }

        public void RemoveEnemy(int enemyIndex)
        {
            EnemyEventData.EnemyData.Remove(enemyIndex);
        }

        public void LoadEnemyEventsData(EventsData data)
        {
            _enemyEventData = data;
            // current index used as last enemy index, for later adding
            if (data.GrassData != null)
                _grassIndex = data.GrassData.Count;
            if (data.EnemyData != null)
                _enemyIndex = data.EnemyData.Count;
            if (data.FishData != null)
                _fishIndex = data.FishData.Count;
            if (data.ObstacleData != null)
                _obstaclesIndex = data.ObstacleData.Count;
            if (data.TargetData != null)
                _targetIndex = data.TargetData.Count;

            LevelEditorControler.CurrentLayerIndex = 0;

            EnviromentMovedSignal.Dispatch();
        }

        public List<EditorEnemyData> AllEnemies
        {
            get => EnemyEventData.EnemyData.Values.ToList();
        }

        public List<EditorEnemyData> GetAllEnemisOnLayer(int layerIndex)
        {
            return EnemyEventData.EnemyData.Values.Where(enemy => enemy.LayerIndex == layerIndex).ToList();
        }

        public EditorEnemyData GetEnemy(int enemyIndex)
        {
            EnemyEventData.EnemyData.TryGetValue(enemyIndex, out EditorEnemyData enemy);
            return enemy;
        }


        //OBSTACLES

        public void AddObstacleEvent(EditorObstacleData data)
        {
            EditorObstacleData dataCopy = new EditorObstacleData(_obstaclesIndex, data.LayerIndex, data.Time, data.StartPos,
                data.StartHeight, data.ScaleX, data.ScaleY, data.MidPos, data.MidHeight, data.EndDirection,
                data.EndRotation, data.Pivot, data.IsPortal);
            EnemyEventData.ObstacleData[_obstaclesIndex] = dataCopy;
            _obstaclesIndex++;
        }

        public void RemoveObstacle(int obstacleIndex)
        {
            EnemyEventData.ObstacleData.Remove(obstacleIndex);
        }

        public List<EditorObstacleData> AllObstacles
        {
            get => EnemyEventData.ObstacleData.Values.ToList();
        }

        public List<EditorObstacleData> GetAllObstaclesOnLayer(int layerIndex)
        {
            return EnemyEventData.ObstacleData.Values.Where(enemy => enemy.LayerIndex == layerIndex).ToList();
        }

        public EditorObstacleData GetObstacle(int obstacleIndex)
        {
            EnemyEventData.ObstacleData.TryGetValue(obstacleIndex, out EditorObstacleData obstacle);
            return obstacle;
        }

        // GRASS
        public void AddGrassEvent(int layerIndex, float time, float posOnLayer)
        {
            EnemyEventData.GrassData[_grassIndex] = new EditorGrassData(_grassIndex, layerIndex, time, posOnLayer);
            _grassIndex++;
        }

        public void AddGrassEvent(EditorGrassData data)
        {
            data.Index = _grassIndex;
            EnemyEventData.GrassData[_grassIndex] = data;
            _grassIndex++;
        }

        public void RemoveGrass(int grassIndex)
        {
            EnemyEventData.GrassData.Remove(grassIndex);
        }

        public List<EditorGrassData> AllGrass
        {
            get => EnemyEventData.GrassData.Values.ToList();
        }

        public List<EditorGrassData> GetAllGrasssOnLayer(int layerIndex)
        {
            return EnemyEventData.GrassData.Values.Where(grass => grass.LayerIndex == layerIndex).ToList();
        }

        public EditorGrassData GetGrass(int grassIndex)
        {
            EnemyEventData.GrassData.TryGetValue(grassIndex, out EditorGrassData grass);

            return grass;
        }

// FISH
        public void AddFishEvent(int layerIndex, float time, float posOnLayer, BombType fishType)
        {
            EnemyEventData.FishData[_fishIndex] =
                new EditorFishData(_fishIndex, layerIndex, time, posOnLayer, fishType);
            _fishIndex++;
        }

        public void AddFishEvent(EditorFishData data)
        {
            data.Index = _fishIndex;
            EnemyEventData.FishData[_fishIndex] = data;
            _fishIndex++;
        }

        public void RemoveFish(int fishIndex)
        {
            EnemyEventData.FishData.Remove(fishIndex);
        }

        public List<EditorFishData> AllFish
        {
            get => EnemyEventData.FishData.Values.ToList();
        }

        public List<EditorFishData> GetAllFishOnLayer(int layerIndex)
        {
            return EnemyEventData.FishData.Values.Where(fish => fish.LayerIndex == layerIndex).ToList();
        }

        public EditorFishData GetFish(int enemyIndex)
        {
            EnemyEventData.FishData.TryGetValue(enemyIndex, out EditorFishData fish);

            return fish;
        }
        
//TARGETS
        public void AddTargetEvent(int layerIndex, float time, float posOnLayer, float depth, TargetType targetType)
        {
            EnemyEventData.TargetData[_targetIndex] =
                new EditorTargetData(_targetIndex, layerIndex, time, posOnLayer, depth, targetType);
            _targetIndex++;
        }

        public void AddTargetEvent(EditorTargetData data)
        {
            data.Index = _targetIndex;
            EnemyEventData.TargetData[_targetIndex] = data;
            _targetIndex++;
        }

        public void RemoveTarget(int targetIndex)
        {
            EnemyEventData.TargetData.Remove(targetIndex);
        }

        public List<EditorTargetData> AllTargets         
        {
            get => EnemyEventData.TargetData.Values.ToList();
        }
        public List<EditorTargetData> GetAllTargetsOnLayer(int layerIndex)
        {
            return EnemyEventData.TargetData.Values.Where(target => target.LayerIndex == layerIndex).ToList();
        }

        public EditorTargetData GetTarget(int targetIndex)
        {
            EnemyEventData.TargetData.TryGetValue(targetIndex, out EditorTargetData target);
            return target;
        }

        public void RemoveEventData(IEditorData data)
        {
            EnemyType enemyType = data.EnemyType;
            int index = data.Index;
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Fish:
                    RemoveFish(index);
                    break;
                case EnemyType.Air:
                    RemoveEnemy(index);
                    break;
                case EnemyType.Grass:
                    RemoveGrass(index);
                    break;
                case EnemyType.Obstacle:
                    RemoveObstacle(index);
                    break;
                case EnemyType.Target:
                    RemoveTarget(index);
                    break;
            }
        }

        public IEditorData GetLastEventData()
        {
           return GetEnemy(EnemyEventData.EnemyData.Count - 1);
        }

        public IEditorData GetEventData(EnemyType enemyType, int index)
        {
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Fish:
                    return GetFish(index);
                case EnemyType.Air:
                    return GetEnemy(index);
                case EnemyType.Grass:
                    return GetGrass(index);
                case EnemyType.Obstacle:
                    return GetObstacle(index);
                case EnemyType.Target:
                    return GetTarget(index);
            }

            return null;
        }

        public List<IEditorData> GetAllEventsOnLayer(int layerIndex)
        {
            List<IEditorData> allData  = new List<IEditorData>();
            allData.AddRange(GetAllEnemisOnLayer(layerIndex));
            allData.AddRange(GetAllFishOnLayer(layerIndex));
            allData.AddRange(GetAllGrasssOnLayer(layerIndex));
            allData.AddRange(GetAllObstaclesOnLayer(layerIndex));
            allData.AddRange(GetAllTargetsOnLayer(layerIndex));
            return allData;
        }

        public void ClearAllData()
        {
            _enemyIndex = 0;
            EnemyEventData.EnemyData.Clear();
            EnemyEventData.FishData.Clear();
            EnemyEventData.GrassData.Clear();
            EnemyEventData.ObstacleData.Clear();
            EnemyEventData.TargetData.Clear();
        }

        private void IndexChangedHandler()
        {
            _visibleLayerIndexes.Clear();
            int currentIndex = LevelEditorControler.CurrentLayerIndex;

            float ratio = LevelEditorControler.ArenaRadius / LevelEditorControler.Bmp;
            int visbleLayers = Mathf.FloorToInt(LevelEditorControler.VisibleRadius / ratio);
            //fix upperBound
            int upperBound = currentIndex + visbleLayers;

            //TODO: make it expandable
            if (upperBound > EnemyEventData.EventLayerData.Count)
            {
                upperBound = EnemyEventData.EventLayerData.Count;
            }

            for (int i = currentIndex; i < upperBound; i++)
            {
                _visibleLayerIndexes.Add(i);
            }

            EnviromentMovedSignal.Dispatch();
        }


        public void SaveChange()
        {
            Debug.Log("SAVE CHANGES !");
            this._enemyEventsBuffer.Add(EnemyEventData.Clone());
        }

        public void Undo()
        {
            var data = this._enemyEventsBuffer.Undo();
            if (data != null)
            {
                _enemyEventData = data;
            }
            else
            {
                Debug.Log("CANT UNDO");
            }
        }

        public void Redo()
        {
            var data = this._enemyEventsBuffer.Redo();
            if (data != null)
            {
                _enemyEventData = data;
            }
            else
            {
                Debug.Log("CANT REDO");
            }
        }
    }
}