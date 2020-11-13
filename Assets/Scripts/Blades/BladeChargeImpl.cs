using UnityEngine;

namespace Blade
{
    public class BladeChargeImpl
    {
        public float RemainingChargeNormalized { get; private set; }

        private float _remainingBladeCharge;
        private float _bladeChargeRate = 1f;
        private float _bladeChargeCapacity = 1f;
        private float _bladeDepleteRate = 0.1f;

        public bool IsFiring = false;

        public void SetFire()
        {
            //IsFiring = !Mathf.Approximately(_remainingBladeCharge, 0);
            IsFiring = false;
        }

        public void Recharge(float normalizedCharge)
        {
            _remainingBladeCharge += normalizedCharge * _bladeChargeRate;
            _remainingBladeCharge = Mathf.Clamp(_remainingBladeCharge, 0, _bladeChargeCapacity);
        }

        public void Update()
        {
            if (IsFiring)
            {
                _remainingBladeCharge = Mathf.Max(0, _remainingBladeCharge - Time.deltaTime * Mathf.Abs(_bladeDepleteRate));
                if (Mathf.Approximately(_remainingBladeCharge, 0))
                {
                    IsFiring = false;
                }
            }
            RemainingChargeNormalized = _remainingBladeCharge / _bladeChargeCapacity;    
        }
    }
}