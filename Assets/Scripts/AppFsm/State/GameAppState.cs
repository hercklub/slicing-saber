using AppFsm.Screen;
using Enemy;
using Framewerk.AppStateMachine;
using UnityEngine;

namespace AppFsm.State
{
    public class GameAppState  : AppState<GameAppScreen>
    {
        [Inject] public EnemyAddedSignal EnemyAddedSignal { get; set; }
        [Inject] public IEnemyFireController EnemyFireController { get; set; }

        
        
        protected override void Enter()
        {
            EnemyAddedSignal.AddListener(EnemyAddedHandler);
            EnemyFireController.Init();
            base.Enter();
        }

        protected override void Exit()
        {
            EnemyAddedSignal.RemoveListener(EnemyAddedHandler);
            EnemyFireController.Destroy();
            base.Exit();
        }

        private void EnemyAddedHandler()
        {
            Screen.InstantiateFlyingEnemyUnit();
            Debug.Log("Enemy Added handler");
        }
        
    }
}