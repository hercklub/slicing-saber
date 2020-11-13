using Contexts;
using Enemy;
using Enemy.Models;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace UI
{
    public class ScoreMediator : Mediator
    {
        [Inject] public IEnemyDataModels EnemyDataModels { get; set; }
        [Inject] public EnemyRemovedSignal EnemyRemovedSignal { get; set; }
        [Inject] public ScoreView View { get; set; }

        public override void OnRegister()
        {
            EnemyRemovedSignal.AddListener(EnemyRemovedHandler);

            View.SetScore(0, 0);
            base.OnRegister();
        }
        
        public override void OnRemove()
        {
            EnemyRemovedSignal.RemoveListener(EnemyRemovedHandler);
            base.OnRemove();
        }
        
        private void EnemyRemovedHandler(int id)
        {
            View.SetScore(EnemyDataModels.SlicedScore, EnemyDataModels.MissedScore);
        }

         
    }
}