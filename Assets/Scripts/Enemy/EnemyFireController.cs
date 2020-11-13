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
        [Inject] public EnemyAddedSignal EnemyAddedSignal { get; set; }
        

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
                EnemyAddedSignal.Dispatch();
                _coolDown = 2f;
            }

            _coolDown -= Time.deltaTime;
        }
    }
}