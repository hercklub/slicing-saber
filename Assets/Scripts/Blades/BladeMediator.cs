using Blade;
using Common;
using Definitions;
using Framewerk;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Blades
{
    public class BladeMediator : Mediator
    {
        [Inject] public BladeView View { get; set; }
        [Inject] public IBladesModel BladesModel { get; set; }
        [Inject] public BladeSelectedSignal BladeSelectSignal { get; set; }
        [Inject] public IInputController InputController { get; set; }

        [Inject] public IUpdater Updater { get; set; }

        private IBladeModel _bladeModel;


        public override void OnRegister()
        {
            base.OnRegister();

            _bladeModel = new BladeModel(View.BladeCollider,
                Vector3.Distance(View.BladeCollider.TopPos.position, View.BladeCollider.BotPos.position));

            BladesModel.AddBlade(View.Hand, _bladeModel);

            View.BladeCollider.OnSliced.AddListener(SlicedHandler);
            BladeSelectSignal.AddListener(BladeSelectedHandler);
            Updater.EveryFrame(UpdateBlade);
        }

        public override void OnRemove()
        {
            Updater.RemoveFrameAction(UpdateBlade);
            View.BladeCollider.OnSliced.RemoveListener(SlicedHandler);
            base.OnRemove();
        }

        private void BladeSelectedHandler(BladeDataDefinitionSO data)
        {
            View.SetBladeColor(data.BladeColor);
        }

        private void SlicedHandler(BladeInteractableView enemy, Vector3 contactPoint, Quaternion orientation,
            Vector3 cutDir)
        {
            if (enemy.IsSlicable)
            {
                enemy.SetSliced(contactPoint, View.Hand, orientation, cutDir);
            }
        }


        private void UpdateBlade()
        {
            Quaternion rotation = Quaternion.LookRotation(InputController.MouseDirection, Vector3.up);
            transform.rotation = rotation;
        }
    }
}