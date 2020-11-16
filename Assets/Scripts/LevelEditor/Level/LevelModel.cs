using System;
using System.Collections.Generic;
using System.Linq;
using LevelEditor;
using LevelEditor.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyAirType
{
    None = 0,
    Normal,
    Collectable,
    Bullet,
    Bomb,
    BombGood
}

[Flags]
public enum eCutDir
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
    All = Left | Right | Up | Down
    
}

public static class CutSideExtension
{
    public static eCutDir AddFlag(this eCutDir a, eCutDir b)
    {
        return a | b;
    }

    public static eCutDir RemoveFlag(this eCutDir a, eCutDir b)
    {
        return a & (~b);
    }

    public static bool HasAxisFlag(this eCutDir a, eCutDir b)
    {
        return (a & b) == b;
    }

    public static eCutDir ToogleFlag(this eCutDir a, eCutDir b)
    {
        return a ^ b;
    }
    
    public static Vector3 ToVector(this eCutDir cutDir)
    {
        Vector3 direction = Vector3.zero;
        switch (cutDir)
        {
            case eCutDir.None:
                break;
            case eCutDir.Left:
                direction = Vector3.left;
                break;
            case eCutDir.Right:
                direction = Vector3.right;
                break;
            case eCutDir.Down:
                direction = Vector3.down;
                break;
            case eCutDir.Up:
                direction = Vector3.up;
                break;

        }

        return direction;
    }
}

public enum EnemyDirection
{
    Down0 = 0,
    Down1,
    Down2,
    Down3,
    
    MidDown0,
    MidDown1,
    MidDown2,
    MidDown3,

    MidU0,
    MidUp1,
    MidUp2,
    MidUp3,
    
    Up0,
    Up1,
    Up2,
    Up3,
}

public enum ObstacleEndDir
{
    DownLeft,
    Down,
    DownRight,
    
    Left,
    Mid,
    Right,
    
    UpLeft,
    Up,
    UpRight
    
    
}

public enum EnemyRotation
{
    None = 0,
    Forward,
    Up,
    Right,
    BackWard,
    Down,
    Left
}

namespace Level
{
    public enum EnemyType
    {
        None = 0,
        Fish,
        Obstacle,
        Target,
        Air,
        Grass
    }



    public interface ILevelModel
    {
        void LoadLevelData(MapSaveData levelData);
        EnemySaveData GetNextEnemyEvent();
        FishSaveData GetNextFishEvent();
        GrassSaveData GetNextGrassEvent();
        ObstacleSaveData GetNextObstacleEvent();
        TargetSaveData GetNextTargetEvent();
        bool IsEnemyEnd();
        bool IsGrassEnd();
        bool IsFishEnd();
        bool IsObstacleEnd();
        bool IsTargetEnd();

        float GetNextPortalTime(float currentTime);
        int LevelId { get; }

        int GetTotalSlicables();
        int GetTotalCollectable();
        int GetTotalObstacles();
        int GetTotalBombs();
        int GetTotalTargets();

    }

    public class LevelModel : ILevelModel
    {
        private Queue<EnemySaveData> _levelEnemyData = new Queue<EnemySaveData>();
        private Queue<FishSaveData> _levelFishData = new Queue<FishSaveData>();
        private Queue<GrassSaveData> _levelGrassData = new Queue<GrassSaveData>();
        private Queue<ObstacleSaveData> _obstacleSaveData = new Queue<ObstacleSaveData>();
        private Queue<TargetSaveData> _targetSaveData = new Queue<TargetSaveData>();

        private List<ObstacleSaveData> _obstaclePortals = new List<ObstacleSaveData>();

        private int difficultyFactor = 0;
        private float _position;
        private int _levelID;

        private int _slicableTotalCount;
        private int _collectableTotalCount;
        private int _bombsTotalCount;
        private int _obstaclesTotalCount;
        private int _targetsTotalCount;
        
        public int LevelId => _levelID;

        public LevelModel(int levelId)
        {
            _levelID = levelId;
        }

        private void AddEnemyEvent(EnemySaveData enemyData)
        {
            _levelEnemyData.Enqueue(enemyData);
        }

        public void AddEnemyEvents(ICollection<EnemySaveData> data)
        {
            _levelEnemyData = new Queue<EnemySaveData>(data);
            _slicableTotalCount = data.Count(e => e.type == EnemyAirType.Normal);
            _collectableTotalCount = data.Count(e => e.type == EnemyAirType.Collectable);
        }

        public void AddFishEvents(ICollection<FishSaveData> data)
        {
            _levelFishData = new Queue<FishSaveData>(data);
            _bombsTotalCount = data.Count();
        }

        public void AddGrassEvents(ICollection<GrassSaveData> data)
        {
            _levelGrassData = new Queue<GrassSaveData>(data);
        }
        
        public void AddObstacleEvents(ICollection<ObstacleSaveData> data)
        {
            _obstacleSaveData = new Queue<ObstacleSaveData>(data);
            _obstaclesTotalCount = data.Count();

            _obstaclePortals = data.Where(obstacle => obstacle.portal).ToList();
        }
        public void AddTargetEvents(ICollection<TargetSaveData> data)
        {
            _targetSaveData = new Queue<TargetSaveData>(data);
            _targetsTotalCount = data.Count;
        }

        public void LoadLevelData(MapSaveData levelData)
        {
            AddEnemyEvents(levelData.enemyEvents);
            AddFishEvents(levelData.fishEvents);
            AddGrassEvents(levelData.grassEvents);
            AddObstacleEvents(levelData.obstacleEvents);
            AddTargetEvents(levelData.targetEvents);
        }

        private float GetNextTime(float currentTime)
        {
            return currentTime + Random.Range(4f, 8f);
        }

        private float GetNextPosition(float currentPosition)
        {
            int sign = Random.value >= 0.5f ? 1 : -1;
            if (currentPosition > 0.1f)
            {
                sign = -1;
            }
            else if (currentPosition < -0.1f)
            {
                sign = 1;
            }

            float offset = Random.Range(0.02f, 0.1f);
            return currentPosition + offset * sign;
        }

        public int GetTotalSlicables()
        {
            return _slicableTotalCount;
        }

        public int GetTotalCollectable()
        {
            return _collectableTotalCount;
        }

        public int GetTotalObstacles()
        {
            return _obstaclesTotalCount;
        }

        public int GetTotalBombs()
        {
            return _bombsTotalCount;
        }
        
        public int GetTotalTargets()
        {
            return _targetsTotalCount;
        }



        public EnemySaveData GetNextEnemyEvent()
        {
            if (!IsEnemyEnd())
            {
                return _levelEnemyData.Dequeue();
            }

            return default(EnemySaveData);
        }

        public FishSaveData GetNextFishEvent()
        {
            if (!IsFishEnd())
            {
                return _levelFishData.Dequeue();
            }

            return default(FishSaveData);
        }

        public GrassSaveData GetNextGrassEvent()
        {
            if (!IsGrassEnd())
            {
                return _levelGrassData.Dequeue();
            }

            return default(GrassSaveData);
        }
        
        public ObstacleSaveData GetNextObstacleEvent()
        {
            if (!IsObstacleEnd())
            {
                return _obstacleSaveData.Dequeue();
            }
            return default(ObstacleSaveData);
        }
        
        public TargetSaveData GetNextTargetEvent()
        {
            if (!IsTargetEnd())
            {
                return _targetSaveData.Dequeue();
            }
            return default(TargetSaveData);
        }

        public bool IsGrassEnd()
        {
            return (_levelGrassData.Count <= 0);
        }

        public bool IsFishEnd()
        {
            return (_levelFishData.Count <= 0);
        }

        public bool IsEnemyEnd()
        {
            return (_levelEnemyData.Count <= 0);
        }
        
        public bool IsObstacleEnd()
        {
            return (_obstacleSaveData.Count <= 0);
        }
        public bool IsTargetEnd()
        {
            return (_targetSaveData.Count <= 0);
        }

        public float GetNextPortalTime(float currentTime)
        {
            var nextPortal = _obstaclePortals.FirstOrDefault(portal => portal.time > currentTime);
            if (nextPortal != null)
            {
                return nextPortal.time;
            }
            return 0f;
        }
    }
}