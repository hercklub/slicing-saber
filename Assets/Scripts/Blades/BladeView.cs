using strange.extensions.mediation.impl;
using UnityEngine;

namespace Blade
{
    public class BladeView : View
    {
        public ControlerHand Hand;
        public BladeCollider BladeCollider;

        public ParticleSystem BladeGoodParticleSystem;

        [SerializeField]
        private ParticleSystem.MainModule BladeMainModule;
        [SerializeField]
        private ParticleSystem.TrailModule BladeTrailsModule;
        
        private float _currentCharge;

        private void Awake()
        {
            BladeMainModule = BladeGoodParticleSystem.main;
            BladeTrailsModule = BladeGoodParticleSystem.trails;
        }

        public void SetBladeColor(Color color)
        {
            BladeTrailsModule.colorOverLifetime = color;
        }

        public void SetChargeProgress(float charge)
        {
            if (!Mathf.Approximately(charge, _currentCharge))
            {
                _currentCharge = charge;
            }
        }
        
    }
}