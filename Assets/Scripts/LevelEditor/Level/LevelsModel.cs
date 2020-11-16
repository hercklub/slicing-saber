using System.Collections.Generic;
using Common;
using LevelEditor;
using LevelEditor.IO;
using UI.Levels;
using UnityEngine;

namespace Level
{
    public interface ILevelsModel
    {
        void AddLevelData(LevelModel enemyData);
        LevelModel GetLevelData(int id);

        void SetSelectedLevel(int levelId);
        LevelModel GetSelectLevel();
        LevelModel[] Levels { get; }
    }

    public class LevelsModel : ILevelsModel
    {
        [Inject] public ILevelImporter LevelImporter { get; set; }

        [Inject] public ILevelInfosModel LevelsInfoModel { get; set; }

        private Dictionary<int, LevelModel> _levelById = new Dictionary<int, LevelModel>();
        public LevelModel[] Levels { get; private set; } = new LevelModel[0];

        private int _selectedLevelId = -1;
        

        public void AddLevelData(LevelModel levelData)
        {
            _levelById[levelData.LevelId] = levelData;
            UpdateLevelssCollection();
        }

        public LevelModel GetLevelData(int id)
        {
            LevelModel level = null;
            _levelById.TryGetValue(id, out level);
            //level?.LoadLevelData(LevelImporter.LoadLevelToGame());
            return level;
        }


        public void SetSelectedLevel(int levelId)
        {
            _selectedLevelId = levelId;
        }

        public LevelModel GetSelectLevel()
        {
            if (_selectedLevelId == -1)
            {
                Debug.LogError("Level was -1 ... not selected");
            }

            var levelInfo = LevelsInfoModel.GetLevelInfo(_selectedLevelId);
            var level = new LevelModel(0);
            level.LoadLevelData(LevelImporter.LoadGameStandardLevel(levelInfo.LevelName));
            return level;
        }
        

        private void UpdateLevelssCollection()
        {
            Levels = new LevelModel[_levelById.Values.Count];
            _levelById.Values.CopyTo(Levels, 0);
        }
    }
}