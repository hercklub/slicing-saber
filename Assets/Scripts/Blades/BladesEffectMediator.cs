using strange.extensions.mediation.impl;

namespace Blade
{
    public class BladesEffectMediator : Mediator
    {
        [Inject] public BladesEffectView View { get; set; }
        [Inject] public IBladesModel BladesModel { get; set; }
        [Inject] public IBladesEffectsImpl BladesEffectsImpl { get; set; }
        
        private bool _sabersAreClashing = false;
        
        private IBladeModel _leftBlade;
        private IBladeModel _rightBlade;
        public override void OnRegister()
        {
            base.OnRegister();

            _leftBlade = BladesModel.GetBlade(ControlerHand.Left);
            _rightBlade = BladesModel.GetBlade(ControlerHand.Right);

            BladesEffectsImpl.SetBlades(_leftBlade.BladeCollider, _rightBlade.BladeCollider);
        }


        public override void OnRemove()
        {
            base.OnRemove();
        }

        private void Update()
        {
            BladesEffectsImpl.UpdateBlades();

            if (BladesEffectsImpl.BladesAreClashing)
            {
                if (!_sabersAreClashing)
                {
                    this._sabersAreClashing = true;
                }
            }
            else if (this._sabersAreClashing)
            {
                this._sabersAreClashing = false;
            }
        }
    }
}