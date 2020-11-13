using Blade;
using Definitions;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Blades
{
    public class BladeMediator : Mediator
    {
        [Inject] public BladeView View { get; set; }
        [Inject] public IBladesModel BladesModel { get; set; }
        [Inject] public BladeSelectSignal BladeSelectSignal { get; set; }

        private IBladeModel _bladeModel;


        public override void OnRegister()
        {
            base.OnRegister();

            _bladeModel = new BladeModel(View.BladeCollider,
                Vector3.Distance(View.BladeCollider.TopPos.position, View.BladeCollider.BotPos.position));
            BladesModel.AddBlade(View.Hand, _bladeModel);

            View.BladeCollider.OnSliced.AddListener(SlicedHandler);

            BladeSelectSignal.AddListener(BladeSelectedHandler);
        }

        private void BladeSelectedHandler(BladeDataDefinitionSO data)
        {
            View.SetBladeColor(data.BladeColor);
        }

        public override void OnRemove()
        {
            View.BladeCollider.OnSliced.RemoveListener(SlicedHandler);
            base.OnRemove();
        }


        private void SlicedHandler(BladeInteractableView enemy, Vector3 contactPoint, Quaternion orientation,
            Vector3 cutDir)
        {
            if (enemy.IsSlicable)
            {
                enemy.SetSliced(contactPoint, View.Hand, orientation, cutDir);
            }
        }


        private void Update()
        {
            if (_bladeModel != null)
            {
                _bladeModel.Update();
                View.SetChargeProgress(_bladeModel.RemainingNormalizedCharge);
            }
        }
    }
}