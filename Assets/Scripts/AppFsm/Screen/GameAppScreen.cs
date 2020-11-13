using Blade;
using Blades;
using Enemies;
using Framewerk.AppStateMachine;
using strange.extensions.pool.api;
using UnityEngine;

namespace AppFsm.Screen
{
    public class GameAppScreen : AppStateScreen
    {
        [Inject] public IPool<BladeInteractableView> EnemyObjectsPool { get; set; }
        
        [Inject] public EnemyInstanceProvider EnemyInstanceProvider { get; set; }
        protected override void Enter()
        {
            base.Enter();
            
            EnemyObjectsPool.instanceProvider = EnemyInstanceProvider;
            
            InstantiateView<BladeListView>();
            InstantiateGamePrefab<BladeView>();
        }

        public void InstantiateFlyingEnemyUnit()
        {
            var flyingEnemy =EnemyObjectsPool.GetInstance();
            Debug.Log("Deploy");
            flyingEnemy.Deploy();
        }
    }
}