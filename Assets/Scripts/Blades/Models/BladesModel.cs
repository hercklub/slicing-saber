using System.Collections.Generic;
using Blades;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Blade
{
    public interface IBladesModel
    {
        void AddBlade(ControlerHand hand, IBladeModel blade);
        IBladeModel GetBlade(ControlerHand hand);
        void AddSkewedEnemy(ControlerHand hand, BladeInteractableObject enemy);
        void RemoveSkewedEnemy(BladeInteractableObject enemy);
        void CleanAllSkewed();
        List<BladeInteractableObject> GetSkewedBy(ControlerHand hand);
        ControlerHand IsSkewedByHand(BladeInteractableObject enemy);
    }

    public class BladesModel : IBladesModel
    {
        
        private List<BladeInteractableObject> _skewedByLeft = new List<BladeInteractableObject>();
        private List<BladeInteractableObject> _skewedByRight = new List<BladeInteractableObject>();
        private Dictionary<ControlerHand, IBladeModel> _bladeByHand = new Dictionary<ControlerHand, IBladeModel>();

        public void AddBlade(ControlerHand hand, IBladeModel blade)
        {
            _bladeByHand[hand] = blade;
        }

        public IBladeModel GetBlade(ControlerHand hand)
        {
            IBladeModel blade = null;
            _bladeByHand.TryGetValue(hand, out blade);
            return blade;
        }

        public void AddSkewedEnemy(ControlerHand hand, BladeInteractableObject enemy)
        {
            if (hand == ControlerHand.Left)
            {
                _skewedByLeft.Add(enemy);
            }
            else
            {
                _skewedByRight.Add(enemy);
            }

        }

        public void RemoveSkewedEnemy(BladeInteractableObject enemy)
        {
            if (_skewedByLeft.Contains(enemy))
            {
                _skewedByLeft.Remove(enemy);
            }
            else if (_skewedByRight.Contains(enemy))
            {
                _skewedByRight.Remove(enemy);
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy} not skewed on any blade!");
            }
        }

        public List<BladeInteractableObject> GetSkewedBy(ControlerHand hand)
        {
            if (hand == ControlerHand.Left)
            {
                return _skewedByLeft;
            }
            else
            {
                return _skewedByRight;
            }
        }

        public void CleanAllSkewed()
        {
            _skewedByLeft.Clear();
            _skewedByRight.Clear();
            GetBlade(ControlerHand.Left).EnergyOrbsCount = 0;
            GetBlade(ControlerHand.Right).EnergyOrbsCount = 0;
        }

        public ControlerHand IsSkewedByHand(BladeInteractableObject enemy)
        {
            if (_skewedByLeft.Contains(enemy))
            {
                return ControlerHand.Left;
            }
            if (_skewedByRight.Contains(enemy))
            {
                return ControlerHand.Right;
            }

            return ControlerHand.None;
        }
    }
}