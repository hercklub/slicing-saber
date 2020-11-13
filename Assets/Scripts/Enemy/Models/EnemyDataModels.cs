using System.Collections.Generic;
using System.Linq;
using strange.extensions.signal.impl;

namespace Enemy.Models
{
    public interface IEnemyModels
    {
        void Init();
        void AddEnemy(EnemyDataModel targetData);
        void RemoveEnemy(int id);
        void RemoveAllEnemies();
        EnemyDataModel GetTarget(int id);
        int NextTargetId { get; }
    }
    
    public class EnemyDataModels : IEnemyModels
    {
        private Dictionary<int, EnemyDataModel> _enemyById = new Dictionary<int, EnemyDataModel>();
        public int NextTargetId { get; private set; }

        public void Init()
        {
            RemoveAllEnemies();
        }

        public void AddEnemy(EnemyDataModel enemyData)
        {
            _enemyById[enemyData.Id] = enemyData;
            NextTargetId++;
        }
        
        public void RemoveEnemy(int id)
        {
            if (_enemyById.ContainsKey(id))
            {
                _enemyById.Remove(id);
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