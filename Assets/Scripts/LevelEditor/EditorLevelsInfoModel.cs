using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelEditor
{
    public class LevelInfoData
    {
        private int _index;
        private string _version;
        private string _levelName = string.Empty;
        private string _displayLevelName = string.Empty;
        private float _beatsPerMinute = 40;
        private int _eventsPerBeat = 4;
        private float _duration;
        private LevelDificulty _difficulty;
        private LevelType _levelType;
        

        public int Index
        {
            get => _index;
        }

        public string Version
        {
            get => _version;
            set => _version = value;
        }

        public string DisplayLevelName
        {
            get => _displayLevelName;
            set => _displayLevelName = value;
        }
        
        public string LevelName
        {
            get => _levelName;
            set => _levelName = value;
        }

        public string LevelFilename
        {
            get => _levelName + ".txt";
        }

        public float BeatsPerMinute
        {
            get => _beatsPerMinute;
            set => _beatsPerMinute = value;
        }
        public int EventsPerBeat
        {
            get => _eventsPerBeat;
            set => _eventsPerBeat = value;
        }
        
        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }
        public LevelDificulty Difficulty
        {
            get => _difficulty;
            set => _difficulty = value;
        }
        
        public LevelType LevelType
        {
            get => _levelType;
            set => _levelType = value;
        }
        


        public LevelInfoData(int index, string version, string levelName, string displayLevelName, float beatsPerMinute, int eventsPerBeat, float duration, LevelDificulty difficulty, LevelType levelType)
        {
            _index = index;
            _version = version;
            _levelName = levelName;
            _displayLevelName = displayLevelName;
            _beatsPerMinute = beatsPerMinute;
            _eventsPerBeat = eventsPerBeat;
            _duration = duration;
            _difficulty = difficulty;
            _levelType = levelType;
        }
        public LevelInfoData(int index)
        {
            _index = index;
        }
    }

    public interface IEditorLevelsInfoModel
    {
        LevelInfoData AddLevelInfo(string version, string levelName, string displayLevelName, float beatsPerMinute,int eventsPerBeat, float duration,
            LevelDificulty difficulty, LevelType levelType);
        LevelInfoData AddEmptyLevelInfo();
        LevelInfoData GetLevelInfo(int index);
        void SetSelectedLevelInfo(int index);
        LevelInfoData GetSelectedLevelInfo();
        List<LevelInfoData> GetAllLevelsInfo { get; }
        
    }

    public class EditorLevelsInfoModel : IEditorLevelsInfoModel
    {
        private Dictionary<int, LevelInfoData> _levelInfoById = new Dictionary<int, LevelInfoData>();

        private int _infoIndex;
        private int _currentLevelIndex;
        
        public LevelInfoData AddLevelInfo(string version, string levelName, string displayLevelName, float beatsPerMinute, int eventsPerBeat, float duration,
            LevelDificulty difficulty, LevelType levelType)
        {
            var levelInfo = new LevelInfoData(_infoIndex, version, levelName,displayLevelName, beatsPerMinute, eventsPerBeat,duration, difficulty, levelType);
            _levelInfoById[_infoIndex] = levelInfo;
            _infoIndex++;
            return levelInfo;
        }

        public LevelInfoData AddEmptyLevelInfo()
        {
            var levelInfo = new LevelInfoData(_infoIndex);
            _levelInfoById[_infoIndex] = levelInfo;
            _infoIndex++;
            return levelInfo;
        }

        public void SetSelectedLevelInfo(int index)
        {
            _currentLevelIndex = index;
        }

        public LevelInfoData GetSelectedLevelInfo()
        {
            return GetLevelInfo(_currentLevelIndex);
        }

        public LevelInfoData GetLevelInfo(int index)
        {
            _levelInfoById.TryGetValue(index, out LevelInfoData levelInfo);
            
            return levelInfo;
        }

        public List<LevelInfoData> GetAllLevelsInfo
        {
            get => _levelInfoById.Values.ToList();
        }
    }
}