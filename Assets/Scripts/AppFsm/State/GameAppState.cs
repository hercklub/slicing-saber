using AppFsm.Screen;
using Common;
using Enemy;
using Framewerk.AppStateMachine;
using UnityEngine;

namespace AppFsm.State
{
    public class GameAppState  : AppState<GameAppScreen>
    {
        [Inject] public EnemyAddedSignal EnemyAddedSignal { get; set; }
        [Inject] public IEnemyFireController EnemyFireController { get; set; }
        [Inject] public IInputController InputController { get; set; }
        
        
        protected override void Enter()
        {
            EnemyAddedSignal.AddListener(EnemyAddedHandler);
            
            EnemyFireController.Init();
            InputController.Init();
            base.Enter();
        }

        protected override void Exit()
        {
            EnemyAddedSignal.RemoveListener(EnemyAddedHandler);
            
            EnemyFireController.Destroy();
            InputController.Destroy();
            base.Exit();
        }

        private void EnemyAddedHandler(int id)
        {
            Screen.InstantiateFlyingEnemyUnit(id);
        }
        
    }
}