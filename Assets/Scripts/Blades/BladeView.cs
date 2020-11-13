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
        private ParticleSystem.TrailModule BladeTrailsModule;
        
        private float _currentCharge;

        private void Awake()
        {
            BladeTrailsModule = BladeGoodParticleSystem.trails;
        }

        public void SetBladeColor(Color color)
        {
            BladeTrailsModule.colorOverLifetime = color;
        }
        
        
    }
}