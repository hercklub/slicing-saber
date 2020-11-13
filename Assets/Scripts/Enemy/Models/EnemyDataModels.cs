using System.Collections.Generic;
using System.Linq;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Enemy.Models
{
    public interface IEnemyDataModels
    {
        void Init();
        void AddEnemy(EnemyDataModel targetData);
        void RemoveEnemy(int id);
        void RemoveAllEnemies();
        EnemyDataModel GetTarget(int id);
        int NextTargetId { get; }

        // todo: move score to separate data model
        void AddScore(bool success);
        int SlicedScore { get; }
        int MissedScore { get; }
    }
    
    public class EnemyDataModels : IEnemyDataModels
    {
        [Inject] public EnemyAddedSignal EnemyAddedSignal { get; set; }
        [Inject] public EnemyRemovedSignal EnemyRemovedSignal { get; set; }
        
        private Dictionary<int, EnemyDataModel> _enemyById = new Dictionary<int, EnemyDataModel>();
        public int NextTargetId { get; private set; }

        public int SlicedScore => _slicedScore;
        public int MissedScore => _missedScore;

        private int _slicedScore;
        private int _missedScore;

        public void Init()
        {
            _slicedScore = 0;
            _missedScore = 0;
            
            RemoveAllEnemies();
        }

        public void AddScore(bool success)
        {
            if (success)
            {
                _slicedScore++;
            }
            else
            {
                _missedScore++;
            }
        }

        public void AddEnemy(EnemyDataModel enemyData)
        {
            _enemyById[enemyData.Id] = enemyData;
            EnemyAddedSignal.Dispatch(enemyData.Id);
            
            NextTargetId++;
        }
        
        public void RemoveEnemy(int id)
        {
            if (_enemyById.ContainsKey(id))
            {
                _enemyById.Remove(id);
                EnemyRemovedSignal.Dispatch(id);
            }
        }

        public void RemoveAllEnemies()
        {
            foreach (var target in _enemyById.ToList())
            {
                RemoveEnemy(target.Value.Id);
            }
        }
        
        public EnemyDataModel GetTarget(int id)
        {
            EnemyDataModel target = null;
            _enemyById.TryGetValue(id, out target);
            return target;
        }
    }
}