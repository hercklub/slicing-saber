using UnityEngine;

namespace Blade
{
    public interface IBladeModel
    {
        BladeCollider BladeCollider { get;  }
        float BladeLength { get;}
    }

    public class BladeModel : IBladeModel
    {
        public BladeCollider BladeCollider { get;}
        public float BladeLength { get;}
        
        private float _energyOrbCount;


        public BladeModel(BladeCollider bladeCollider, float bladeLength)
        {
            BladeCollider = bladeCollider;
            BladeLength = bladeLength;
        }



    }
}