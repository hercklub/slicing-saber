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
        [Inject] public IBladesDataDefinitions BladesDataDefinitions { get; set; }
        [Inject] public BladeSelectSignal BladeSelectSignal { get; set; }

        private IBladeModel _bladeModel;


        public override void OnRegister()
        {
            base.OnRegister();

            _bladeModel = new BladeModel(View.BladeCollider, Vector3.Distance(View.BladeCollider.TopPos.position, View.BladeCollider.BotPos.position));
            BladesModel.AddBlade(View.Hand, _bladeModel);
            
            View.BladeCollider.OnSliced.AddListener(SlicedHandler);
            View.BladeCollider.OnSkewed.AddListener(SkewedHandler);
            
            BladeSelectSignal.AddListener(BladeSelectedHandler);
        }

        private void BladeSelectedHandler(BladeDataDefinitionSO data)
        {
            View.SetBladeColor(data.BladeColor);
        }

        public override void OnRemove()
        {
            View.BladeCollider.OnSliced.RemoveListener(SlicedHandler);
            View.BladeCollider.OnSkewed.RemoveListener(SkewedHandler);
            base.OnRemove();
        }


        private void SkewedHandler(BladeInteractableObject enemy)
        {
            if (enemy.IsSkewed)
            {
                return;
            }

            enemy.SetSkewed(true, View.Hand);
            BladesModel.AddSkewedEnemy(View.Hand, enemy);
        }


        private void SlicedHandler(BladeInteractableObject enemy, Vector3 contactPoint, Quaternion orientation,
            Vector3 cutDir)
        {
            if (enemy.IsSkewed)
            {
                if (BladesModel.IsSkewedByHand(enemy) == View.Hand)
                {
                    return;
                }
            }

            if (enemy.IsBomb)
            {
                enemy.SetBounced(contactPoint, View.Hand, orientation, cutDir);
            }
            else
            {
                enemy.SetSliced(contactPoint, View.Hand, orientation, cutDir);
            }

            if (enemy.IsSkewed)
            {
                enemy.SetSkewed(false, View.Hand);
                BladesModel.RemoveSkewedEnemy(enemy);
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

        private float GetCurrentScoredBoxes()
        {
            return _bladeModel.EnergyOrbsCount;
        }


        private void FixedUpdate()
        {
            var bladeRotationFactor = -Vector3.Dot(View.transform.forward, Vector3.up);
            var _skewedBoxes = BladesModel.GetSkewedBy(View.Hand);
            
            
            for (int i = 0; i < _skewedBoxes.Count; i++)
            {
                if (_skewedBoxes[i].IsCollectable)
                {
                    var diff = View.BladeCollider.TopPos.position - View.BladeCollider.BotPos.position;
                    var position = View.BladeCollider.BotPos.position +
                                   _skewedBoxes[i].ProgressAlongBlade * diff;

                    float prevProgressAlongBlade = _skewedBoxes[i].ProgressAlongBlade;
                    float speed = _skewedBoxes[i].SlidingSpeed;
                    _skewedBoxes[i].ProgressAlongBlade += bladeRotationFactor * speed * Time.deltaTime;
                    _skewedBoxes[i].ProgressAlongBlade = Mathf.Clamp01(_skewedBoxes[i].ProgressAlongBlade);

                    float newSpeed = Mathf.Abs((_skewedBoxes[i].ProgressAlongBlade - prevProgressAlongBlade) /
                                               Time.deltaTime);
                    _skewedBoxes[i].SlidingSpeed = Mathf.Lerp(_skewedBoxes[i].SlidingSpeed, newSpeed,
                        (!(newSpeed < _skewedBoxes[i].SlidingSpeed)) ? 4f : (Time.deltaTime * 16f));

                    _skewedBoxes[i].transform.position = position;

                    if (_skewedBoxes[i].ProgressAlongBlade <= 0)
                    {
                        if (_skewedBoxes[i].SetSkewedReachedEnd(View.Hand))
                        {
                            BladesModel.RemoveSkewedEnemy(_skewedBoxes[i]);
                            _bladeModel.RechargeBlade(0.1f);
                        }
                    }
                }
            }
        }
    }
}