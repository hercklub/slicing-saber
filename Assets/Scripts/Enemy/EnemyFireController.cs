using Enemy.Models;
using Framewerk;
using UnityEngine;

namespace Enemy
{
    public interface IEnemyFireController
    {
        void Init();
        void Destroy();
    }

    public class EnemyFireController : IEnemyFireController
    {
        [Inject] public IUpdater Updater { get; set; }
        [Inject] public IEnemyDataModels EnemyDataModels { get; set; }

        private float _coolDown = 2f;

        public void Init()
        {
            Updater.EveryFrame(UpdateEnemies);
        }

        public void Destroy()
        {
            Updater.RemoveFrameAction(UpdateEnemies);
        }

        public void UpdateEnemies()
        {
            if (_coolDown <= 0f)
            {
                int enemyId = EnemyDataModels.NextTargetId;
                //todo: bunch of magic constants, move to config
                // randomize start pos X coordinate
                Vector3 startPos = Vector3.forward * 6f + Vector3.right * (Random.value * 2f - 1f);
                // dir calculated form start pos toward blade + added randomized X coordinate
                Vector3 startDir = (-startPos + Vector3.up * 4f + Vector3.right * (0.5f * (Random.value * 2f - 1f))).normalized;
                EnemyDataModels.AddEnemy(new EnemyDataModel(enemyId, startPos, startDir, 0.8f));
                _coolDown = Random.Range(0.2f, 2f);
            }

            _coolDown -= Time.deltaTime;
        }
    }
}