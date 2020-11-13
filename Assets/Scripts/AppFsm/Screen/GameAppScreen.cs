using Arena;
using Blade;
using Blades;
using Enemies;
using Framewerk.AppStateMachine;
using strange.extensions.pool.api;
using UI;
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
            InstantiateView<ScoreView>();
            
            InstantiateGamePrefab<BladeView>();
            InstantiateGamePrefab<ArenaView>();
        }

        public void InstantiateFlyingEnemyUnit(int id)
        {
            var flyingEnemy =EnemyObjectsPool.GetInstance();
            flyingEnemy.Id = id;
            flyingEnemy.Deploy();
        }
    }
}