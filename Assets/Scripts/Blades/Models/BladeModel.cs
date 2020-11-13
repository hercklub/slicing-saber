using UnityEngine;

namespace Blade
{
    public interface IBladeModel
    {
        BladeCollider BladeCollider { get;  }
        float BladeLength { get;}
        void Update();
        void RechargeBlade(float normalizedEnergy);
        float RemainingNormalizedCharge { get; }
        float EnergyOrbsCount { get; set; }
        
    }

    public class BladeModel : IBladeModel
    {
        public BladeCollider BladeCollider { get;}
        public float BladeLength { get;}
        private BladeChargeImpl _bladeChargeImpl;
        private float _energyOrbCount;
        public float EnergyOrbsCount
        {
            get => _energyOrbCount;
            set => _energyOrbCount = value;
        }

        public float RemainingNormalizedCharge => _bladeChargeImpl.RemainingChargeNormalized;
        public float BladeSpeed => BladeCollider.BladeSpeed;
        public float BladeSpeedNormalized => Mathf.Clamp01(BladeCollider.BladeSpeed / 30f);

        public bool IsEnabled { get; set; }

        public BladeModel(BladeCollider bladeCollider, float bladeLength)
        {
            BladeCollider = bladeCollider;
            BladeLength = bladeLength;
            _bladeChargeImpl = new BladeChargeImpl();
        }

        public void RechargeBlade(float normalizedEnergy)
        {
            _bladeChargeImpl.Recharge(normalizedEnergy);
        }

        public void SetFiring()
        {
            _bladeChargeImpl.SetFire();
        }

        public void Update()
        {
            _bladeChargeImpl.Update();
        }
    }
}