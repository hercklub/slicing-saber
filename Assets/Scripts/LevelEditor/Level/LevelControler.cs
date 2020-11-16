using BehindYou.Scripts.Controller;
using Framewerk;
using LevelEditor;
using UI.Levels;
using UnityEngine;

namespace Level
{
    public interface ILevelControler
    {
        void Init();
        void LevelEnd(bool completed);
        float CurrentLevelTime { get; }
        bool LevelCompleted { get; }
        ILevelModel CurrentLevel { get; }
        float TimeMultiplier { get; set; }
    }

    public class LevelControler : ILevelControler
    {
        [Inject] public ILevelsModel LevelsModel { get; set; }
        [Inject] public IPortalControler PortalControler { get; set; }
        [Inject] public IUpdater Updater { get; set; }
        [Inject] public ILevelInfosModel LevelInfosModel { get; set; }

        public float CurrentLevelTime => _levelTime;
        public bool LevelCompleted => _levelCompeleted;

        public ILevelModel CurrentLevel => _currentLevel;

        private float _startTime;
        
        private EnemySaveData _currentEnemyData;
        private GrassSaveData _currentGrasData;
        private FishSaveData _currentFishData;
        private ObstacleSaveData _currentobstacleData;
        private TargetSaveData _currentTargetData;

        private float _timeOffset = 0f;
        private ILevelModel _currentLevel;

        private float _timeToReachPlayer = 0f;
        private float _levelTime;
        private bool _levelCompeleted;
        private bool _levelPaused = false;

        
        private float _bpm = 1;
        private int _eventsPerBeat = 1;
        private float _step;
        
        
        private bool _isLevelEnd = false;
        private float _timeToEnd = 0f;

        public float TimeMultiplier
        {
            get => _timeMultiplier;
            set => _timeMultiplier = value;
        }

        private float _timeMultiplier = 1f;
        private float _baseTimeMultiplier = 1f;
        private float _nextPortalTime = float.MaxValue;


        public void Init()
        {
            LevelEnd(false);

            _currentLevel = LevelsModel.GetSelectLevel();
            _currentEnemyData = _currentLevel.GetNextEnemyEvent();
            _currentGrasData = _currentLevel.GetNextGrassEvent();
            _currentFishData = _currentLevel.GetNextFishEvent();
            _currentobstacleData = _currentLevel.GetNextObstacleEvent();
            _currentTargetData = _currentLevel.GetNextTargetEvent();


            _levelCompeleted = false;

            Updater.EveryStep(UpdateLevel);

            var levelInfo = LevelInfosModel.GetSelectedLevelInfo();
            _bpm = levelInfo.BeatsPerMinute;
            _eventsPerBeat = levelInfo.EventsPerBeat;

            _step = _bpm * _eventsPerBeat / 60f;

            float radius = 40;
            float numOfDivision = radius / (radius / _bpm);


            float timePerDivision = 60f / (_bpm * _eventsPerBeat);

            // this is the time until the end of path
            _timeToReachPlayer = (numOfDivision - 1) * timePerDivision;
            PortalControler.PortalDidPassedSignal.AddListener(PortalDidPassedHandler);
            
            _timeMultiplier = _baseTimeMultiplier;

        }


        public void LevelEnd(bool isCompleted)
        {
            _levelCompeleted = isCompleted;
            _levelPaused = false;
            Updater.RemoveStepAction(UpdateLevel);
            
            PortalControler.PortalDidPassedSignal.RemoveListener(PortalDidPassedHandler);
        }

        private void LevelPauseHandler(bool paused)
        {
            LevelPaused(paused);
        }

        private void LevelPaused(bool paused)
        {
            if (paused && !_levelPaused)
            {
                Updater.RemoveStepAction(UpdateLevel);
                _levelPaused = paused;
            }

            if (!paused && _levelPaused)
            {
                Updater.EveryStep(UpdateLevel);
                _levelPaused = paused;
            }
        }
        

        private void PortalDidPassedHandler(Dimension dimension)
        {
            if (dimension == Dimension.Evil)
            {
                _timeMultiplier = _baseTimeMultiplier * 1.3f;
                _nextPortalTime = _currentLevel.GetNextPortalTime(_levelTime * _step) / _step;
            }
            else
            {
                _timeMultiplier = _baseTimeMultiplier;
                _nextPortalTime = float.MaxValue;
            }
        }

        private void UpdateLevel()
        {
            _levelTime += Time.deltaTime * _timeMultiplier;

            if (_currentEnemyData != null)
            {
                float enemyTime = _currentEnemyData.time;
                enemyTime = enemyTime / _step;
                // enemies
                if (_levelTime >= enemyTime - _timeOffset)
                {
                    if (enemyTime - _timeOffset >= 0)
                    {
                        // launch enemy
                    }

                    _currentEnemyData = _currentLevel.GetNextEnemyEvent();
                }
            }

            if (_currentFishData != null)
            {
                //fish
                float fishTime = _currentFishData.time;
                fishTime = fishTime / _step;
                if (_levelTime >= fishTime - _timeOffset + _timeToReachPlayer)
                {
                    if (fishTime - _timeOffset >= 0)
                    {
                        // launch fish
                    }

                    _currentFishData = _currentLevel.GetNextFishEvent();
                }
            }

            if (_currentGrasData != null)
            {
                // grass
                float grassTime = _currentGrasData.time;
                grassTime = grassTime / _step;
                if (_levelTime >= grassTime - _timeOffset + _timeToReachPlayer)
                {
                    if (grassTime - _timeOffset >= 0)
                    {
                        // launch grass
                    }

                    _currentGrasData = _currentLevel.GetNextGrassEvent();
                }
            }

            if (_currentobstacleData != null)
            {
                float obstacleTime = _currentobstacleData.time;
                obstacleTime = obstacleTime / _step;
                // obstacles
                if (_levelTime >= obstacleTime - _timeOffset)
                {
                    if (obstacleTime - _timeOffset >= 0)
                    {
                        // launch obstacle
                    }

                    _currentobstacleData = _currentLevel.GetNextObstacleEvent();
                }
            }

            if (_currentTargetData != null)
            {
                // grass
                float targetTime = _currentTargetData.time;
                targetTime = targetTime / _step;
                if (_levelTime >= targetTime - _timeOffset + _timeToReachPlayer)
                {
                    if (targetTime - _timeOffset >= 0)
                    {
                        // launch target
                    }

                    _currentTargetData = _currentLevel.GetNextTargetEvent();
                }
            }

        }

    }
    
}