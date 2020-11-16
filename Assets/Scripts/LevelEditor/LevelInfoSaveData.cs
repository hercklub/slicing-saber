using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    public enum LevelDificulty
    {
        Easy,
        Normal,
        Hard,
        Expert
    }

    public enum LevelType
    {
        Normal,
        Challenge,
        Tutorial,
        Boss
    }

    [Serializable]
    public class LevelInfoSaveData
    {
        private const string CURRENT_VERSION = "0.0.2";

        [SerializeField] private string _version;
        [SerializeField] private string _levelName;
        [SerializeField] private string _displayLevelName;
        [SerializeField] private float _beatsPerMinute;
        [SerializeField] private int _eventsPerBeat;
        [SerializeField] private float _duration;
        [SerializeField] private LevelDificulty _difficulty;
        [SerializeField] private LevelType _levelType;

        public string Version
        {
            get => _version;
        }

        public string LevelName
        {
            get => _levelName;
        }

        public string DisplayLevelName
        {
            get => _displayLevelName;
        }
        
        public float BeatsPerMinute
        {
            get => _beatsPerMinute;
        }
        public int EventsPerBeat
        {
            get => _eventsPerBeat;
        }
        
        public float Duration
        {
            get => _duration;
        }

        public LevelDificulty Difficulty
        {
            get => _difficulty;
        }
        
        public LevelType LevelType
        {
            get => _levelType;
        }

 
        public bool HasAllData
        {
            get
            {
                return this._version != null && this._levelName != null  && (double) this._beatsPerMinute != 0.0;
            }
        }

        public LevelInfoSaveData(string levelName, string displayLevelName, float beatsPerMinute, int eventsPerBeat, float duration, LevelDificulty difficulty, LevelType levelType)
        {
            _version = CURRENT_VERSION;
            _levelName = levelName;
            _displayLevelName = displayLevelName;
            _beatsPerMinute = beatsPerMinute;
            _eventsPerBeat = eventsPerBeat;
            _duration = duration;
            _difficulty = difficulty;
            _levelType = levelType;
        }

        public static LevelInfoSaveData DeserializeFromJSONString(string stringData)
        {
            LevelInfoSaveData levelInfoSaveData = (LevelInfoSaveData) null;
            
            try
            {
                levelInfoSaveData = JsonUtility.FromJson<LevelInfoSaveData>(stringData);
            }
            catch
            {
                Debug.Log("Cant read LEVEL INFO !");
            }
            
            return levelInfoSaveData;
        }
        public string SerializeToJSONString()
        {
            return JsonUtility.ToJson((object) this);
        }
    }
}