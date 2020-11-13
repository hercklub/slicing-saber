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

    }

    public class BladesModel : IBladesModel
    {
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

    }
}