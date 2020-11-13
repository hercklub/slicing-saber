using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Blades
{
    public interface IBladeInteractable
    {
        bool IsSkewed { get; }
        void SetSliced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir);
        void SetBounced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir);
        void SetSkewed(bool isSkewed, ControlerHand hand);

        bool SetSkewedReachedEnd(ControlerHand hand);
        Signal<ControlerHand> OnSkewed { get; }
        Signal<ControlerHand> OnSkewedReachedEnd { get; }

    }

    public class BladeInteractableObject : View, IBladeInteractable
    {
        public bool IsBomb;
        public bool IsSlicable;
        public bool IsCollectable;
        public float SlidingSpeed = 2f;
        
        
        private bool _isSkewed;
        private float _progressAlongBlade;
        public bool IsSkewed => _isSkewed;

        public Signal<ControlerHand> OnSkewed { get; } = new Signal<ControlerHand>();
        public Signal<ControlerHand> OnSkewedReachedEnd { get; } = new Signal<ControlerHand>();

        public float ProgressAlongBlade
        {
            get => _progressAlongBlade;
            set => _progressAlongBlade = value;
        }


        public void SetSkewed(bool isSkewed, ControlerHand hand)
        {
            if (isSkewed)
            {
                OnSkewed.Dispatch(hand);
            }
            
            _isSkewed = isSkewed;
        }

        public bool SetSkewedReachedEnd(ControlerHand hand)
        {
            if (!IsBomb)
            {
                OnSkewedReachedEnd.Dispatch(hand);
                return true;
            }
            return false;
        }
        
        public void SetSliced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir)
        {
        }

        public void SetBounced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir)
        {
        }
    }
}