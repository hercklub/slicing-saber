using UnityEngine;

namespace Blade
{
    public interface IBladeModel
    {
        BladeCollider BladeCollider { get;  }
        float BladeLength { get;}
        void Update();
        float RemainingNormalizedCharge { get; }
        
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


        public BladeModel(BladeCollider bladeCollider, float bladeLength)
        {
            BladeCollider = bladeCollider;
            BladeLength = bladeLength;
            _bladeChargeImpl = new BladeChargeImpl();
        }


        public void Update()
        {
            _bladeChargeImpl.Update();
        }
    }
}