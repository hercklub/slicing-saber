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
        [Inject] public ScoreView View { get; set; }

        public override void OnRegister()
        {

            View.SetScore(0, 0);
            base.OnRegister();
        }
        
        
        [ListensTo(typeof(EnemyRemovedSignal))]
        private void EnemyRemovedHandler(int id)
        {
            View.SetScore(EnemyDataModels.SlicedScore, EnemyDataModels.MissedScore);
        }

         
    }
}