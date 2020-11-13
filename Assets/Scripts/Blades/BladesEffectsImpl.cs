using Framewerk;
using UnityEngine;

namespace Blade
{
    public interface IBladesEffectsImpl
    {
       void UpdateBlades();
       bool  BladesAreClashing { get; }
       void SetBlades(BladeCollider leftBlade, BladeCollider rightBlade);
    }

    public class BladesEffectsImpl : IBladesEffectsImpl
    {
        private BladeCollider _leftBlade;
        private BladeCollider _rightBlade;

        private bool _bladesAreClashing = false;
        private bool _bladeClashImmediate = false;

        private Vector3 _clasingPoint;

        protected float _minDistanceToClash = 0.08f;
        private float _clashignCooldown = 0f;
        private float _clashingTimer = 0f;

        public bool BladesAreClashing => _bladesAreClashing;
        public Vector3 ClashingPoint => _clasingPoint;

        public void SetBlades(BladeCollider leftBlade, BladeCollider rightBlade)
        {
            _leftBlade = leftBlade;
            _rightBlade = rightBlade;
        }
        
        public void UpdateBlades()
        {
            Vector3 saberBladeTopPos = _leftBlade.TopPos.position;
            Vector3 saberBladeTopPos2 = _rightBlade.TopPos.position;
            Vector3 saberBladeBottomPos = _leftBlade.BotPos.position;
            Vector3 saberBladeBottomPos2 = _rightBlade.BotPos.position;

            if (saberBladeBottomPos == saberBladeBottomPos2)
            {
                this._bladeClashImmediate = false;
            }
            else
            {
                Vector3 clashingPoint = Vector3.zero;
                float distances = 0f;
                if (distances < this._minDistanceToClash)
                {
                    _clasingPoint = clashingPoint;
                    this._bladeClashImmediate = true;
                }
                else
                {
                    this._bladeClashImmediate = false;
                }
            }

            if (_bladeClashImmediate)
            {
                if (_clashingTimer > _clashignCooldown)
                {
                    _bladesAreClashing = true;
                }
                _clashingTimer += Time.deltaTime;
            }
            else
            {
                _clashingTimer = 0f;
                _bladesAreClashing = false;
            }
        }
        
    }
}