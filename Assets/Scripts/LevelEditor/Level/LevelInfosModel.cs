using System.Collections.Generic;
using System.Linq;
using LevelEditor;
using UnityEngine;

namespace UI.Levels
{
    public interface ILevelInfosModel
    {
        LevelInfoData AddLevelInfo(string version, string levelName, string displayLevelName, float beatsPerMinute, int eventsPerBeat, float duration,
            LevelDificulty difficulty, LevelType levelType);
        List<LevelInfoData> GetAllLevelsInfo { get; }
        List<LevelInfoData> GetChallengesLevelsInfo { get; }
        List<LevelInfoData> GetPlayableLevels { get; }
        LevelInfoData GetLevelInfo(int index);
        
        void SetSelectedLevelInfo(int index);
        LevelInfoData GetSelectedLevelInfo();
    }

    public class LevelInfosModel : ILevelInfosModel
    {
        private Dictionary<int, LevelInfoData> _levelInfoById = new Dictionary<int, LevelInfoData>();

        private string[] _playableLevels = new[] {  "Level8.8", "Level5.5", "Level6.6", "Level7.7", "Level4.4", "Level2.2", "Level9.9", "Level1.1", "Level3.3", "BulletTest", "Empty", "TargetTest"};
        private int _infoIndex;
        private int _currentLevelIndex = -1;

        public LevelInfoData AddLevelInfo(string version, string levelName, string displayLevelName, float beatsPerMinute, int eventsPerBeat, float duration, LevelDificulty difficulty, LevelType levelType)
        {
            var levelInfo = new LevelInfoData(_infoIndex, version, levelName, displayLevelName, beatsPerMinute, eventsPerBeat, duration, difficulty, levelType);
            _levelInfoById[_infoIndex] = levelInfo;
            _infoIndex++;
            return levelInfo;
        }

        public List<LevelInfoData> GetAllLevelsInfo
        {
            get => _levelInfoById.Values.ToList(); 
            
        }

        public List<LevelInfoData> GetChallengesLevelsInfo 
        {
            get
            {
                return _levelInfoById.Values.Where(l => l.LevelType == LevelType.Challenge).ToList();
            }
            
        }
        public List<LevelInfoData> GetPlayableLevels
        {
            get
            {
                List<LevelInfoData> levelInfos = new List<LevelInfoData>();
                for (int i = 0; i < _playableLevels.Length; i++)
                {
                    foreach (var level in _levelInfoById)
                    {
                        if (string.Compare(level.Value.LevelName, _playableLevels[i]) == 0)
                        {
                            levelInfos.Add(level.Value);
                        }
                    }
                }

                return levelInfos;
            }
        }

        public LevelInfoData GetLevelInfo(int index)
        {
            _levelInfoById.TryGetValue(index, out LevelInfoData levelInfo);
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

    }
}