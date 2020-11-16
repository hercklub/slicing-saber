using Level;

namespace LevelEditor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    [Serializable]
    public class MapSaveData
    {
        private const string CURRENT_VERSION = "0.0.4";
        [SerializeField] private string _version;
        [SerializeField] private List<EnemySaveData> _enemyEvents;
        [SerializeField] private List<GrassSaveData> _grassEvents;
        [SerializeField] private List<FishSaveData> _fishEvents;
        [SerializeField] private List<ObstacleSaveData> _obstacleEvents;
        [SerializeField] private List<TargetSaveData> _targetEvents;

        public string version
        {
            get { return this._version; }
        }

        public List<EnemySaveData> enemyEvents
        {
            get { return this._enemyEvents; }
        }
        public List<GrassSaveData> grassEvents
        {
            get { return this._grassEvents; }
        }
        public List<FishSaveData> fishEvents
        {
            get { return this._fishEvents; }
        }
        
        public List<ObstacleSaveData> obstacleEvents
        {
            get { return this._obstacleEvents; }
        }
        public List<TargetSaveData> targetEvents
        {
            get { return this._targetEvents; }
        }


        public MapSaveData(List<EnemySaveData> enemyEvents, List<GrassSaveData> grassEvents, List<FishSaveData> fishEvents, List<ObstacleSaveData> obstacleEvents, List<TargetSaveData> targetEvents)
        {
            this._version = CURRENT_VERSION;
            this._enemyEvents = enemyEvents;
            this._grassEvents = grassEvents;
            this._fishEvents = fishEvents;
            this._obstacleEvents = obstacleEvents;
            this._targetEvents = targetEvents;
        }


        public string SerializeToJSONString()
        {
            return JsonUtility.ToJson((object) this);
        }

        public static MapSaveData DeserializeFromJSONString(string stringData)
        {
            MapSaveData mapSaveData = JsonUtility.FromJson<MapSaveData>(stringData);
            return mapSaveData;
        }
        

        public interface ITime
        {
            float time { get; }
        }


    }
    
    [Serializable]
    public class EnemySaveData : MapSaveData.ITime
    {
        [SerializeField] private float _time;
        [SerializeField] private float _spos;
        [SerializeField] private float _sheight;
        [SerializeField] private EnemyAirType _type;
        
        [SerializeField] private float _mpos;
        [SerializeField] private float _mheight;
        [SerializeField] private EnemyRotation _mrot;
        
        [SerializeField] private EnemyRotation _erot;
        [SerializeField] private EnemyDirection _edir;
        [SerializeField] private eCutDir _cutDir;

        

        public float time
        {
            get { return this._time; }
            set { this._time = value; }

        }

        public float spos
        {
            get { return this._spos; }
        }

        public float sheight
        {
            get { return this._sheight; }
        }

        public EnemyAirType type
        {
            get { return this._type; }
        }
        
        public float mpos
        {
            get { return this._mpos; }
        }

        public float mheight
        {
            get { return this._mheight; }
        }
        public EnemyRotation mrot
        {
            get { return this._mrot; }
        }

        public EnemyRotation erot
        {
            get { return this._erot; }
        }
        public EnemyDirection edir
        {
            get { return this._edir; }
        }

        public eCutDir cutdir
        {
            get { return this._cutDir; }
        }

        public EnemySaveData(float time, float spos, float sheight, EnemyAirType type, float mpos, float mheight, EnemyRotation mrot, EnemyRotation erot, EnemyDirection edir, eCutDir cutDir)
        {
            _time = time;
            _spos = spos;
            _sheight = sheight;
            _type = type;
            _mpos = mpos;
            _mheight = mheight;
            _mrot = mrot;
            _erot = erot;
            _edir = edir;
            _cutDir = cutDir;
        }

        public void MoveTime(float offset)
        {
            this._time += offset;
        }
    }
    
    [Serializable]
    public class ObstacleSaveData : MapSaveData.ITime
    {
        [SerializeField] private float _time;
        [SerializeField] private float _spos;
        [SerializeField] private float _sheight;

        [SerializeField] private float _scaleX;
        [SerializeField] private float _scaleY;
        
        [SerializeField] private float _mpos;
        [SerializeField] private float _mheight;
        
        [SerializeField] private ObstacleEndDir _edir;
        [SerializeField] private float _erot;
        
        
        [SerializeField] private ObstacleEndDir _pivot;
        [SerializeField] private bool _portal;
        

        public float time
        {
            get { return this._time; }
            set { this._time = value; }
        }

        public float spos
        {
            get { return this._spos; }
        }

        public float sheight
        {
            get { return this._sheight; }
        }
        
        
        public float scaleX
        {
            get { return this._scaleX; }
        }

        public float scaleY
        {
            get { return this._scaleY; }
            set { this._scaleY = value; }
        }

        public float mpos
        {
            get { return this._mpos; }
        }
        public float mheight
        {
            get { return this._mheight; }
        }
        
        public float erot
        {
            get { return this._erot; }
        }
        public ObstacleEndDir edir
        {
            get { return this._edir; }
        }
        
        public ObstacleEndDir pivot
        {
            get { return this._pivot; }
        }
        public bool portal
        {
            get { return this._portal; }
        }

        public ObstacleSaveData(float time, float spos, float sheight, float scaleX, float scaleY, float mpos, float mheight, ObstacleEndDir edir, float erot, ObstacleEndDir pivot, bool portal)
        {
            _time = time;
            _spos = spos;
            _sheight = sheight;
            _scaleX = scaleX;
            _scaleY = scaleY;
            _mpos = mpos;
            _mheight = mheight;
            _edir = edir;
            _erot = erot;
            _pivot = pivot;
            _portal = portal;
        }

        public void MoveTime(float offset)
        {
            this._time += offset;
        }
    }
    [Serializable]
    public class GrassSaveData : MapSaveData.ITime
    {
        [SerializeField] private float _time;
        [SerializeField] private float _pos;

        public float time
        {
            get { return this._time; }
            set { this._time = value; }
        }

        public float pos
        {
            get { return this._pos; }
        }


        public GrassSaveData(float time, float pos)
        {
            _time = time;
            _pos = pos;
        }
    }
    [Serializable]
    public class FishSaveData : MapSaveData.ITime
    {
        [SerializeField] private float _time;
        [SerializeField] private float _pos;
        [SerializeField] private BombType _fishType;

        public float time
        {
            get { return this._time; }
            set { this._time = value; }
        }

        public float pos
        {
            get { return this._pos; }
        }
        
        public BombType fishType
        {
            get { return this._fishType; }
        }

        public FishSaveData(float time, float pos, BombType fishType)
        {
            _time = time;
            _pos = pos;
            _fishType = fishType;

        }
    }
    
    [Serializable]
    public class TargetSaveData : MapSaveData.ITime
    {
        [SerializeField] private float _time;
        [SerializeField] private float _pos;
        [SerializeField] private float _depth;
        [SerializeField] private TargetType _targetType;

        public float time
        {
            get { return this._time; }
            set { this._time = value; }
        }

        public float pos
        {
            get { return this._pos; }
        }
        public float depth
        {
            get { return this._depth; }
        }
        
        public TargetType targetType
        {
            get { return this._targetType; }
        }

        public TargetSaveData(float time, float pos, float depth, TargetType targetType)
        {
            _time = time;
            _pos = pos;
            _depth = depth;
            _targetType = targetType;

        }
    }
}