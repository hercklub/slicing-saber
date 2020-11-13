using Blades;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Blade
{
    public class BladeCollider : View
    {
        public BladeData LastData;

        public Transform BotPos;
        public Transform TopPos;
        
        public float PokeAngle = 0.1f;
        public float PokeEndPointPercentage = 0.7f;
        
        public float BombPokeAngle = 0.1f;
        public float BombPokeEndPointPercentage = 0.8f;

        public float BladeSpeed;

        private Collider[] _colliders = new Collider[16];

        public Signal<BladeInteractableObject> OnSkewed = new Signal<BladeInteractableObject>();

        public Signal<BladeInteractableObject, Vector3, Quaternion, Vector3> OnSliced =
            new Signal<BladeInteractableObject, Vector3, Quaternion, Vector3>();

        private BladeData _currentData;

        void FixedUpdate()
        {
            _currentData = new BladeData(BotPos.position, TopPos.position, transform.rotation, transform.forward);

            float newSpeed = ((_currentData.topPos - LastData.topPos) / Time.deltaTime).magnitude;
            BladeSpeed = Mathf.Lerp(this.BladeSpeed, newSpeed, (!(newSpeed < this.BladeSpeed)) ? 4f : (Time.deltaTime * 16f));

            Cut(_currentData.topPos, _currentData.botPos, LastData.botPos, LastData.topPos);
        }

        private void Cut(Vector3 topPos, Vector3 bottomPos, Vector3 prevBottomPos, Vector3 prevTopPos)
        {
            Vector3 vector;
            Vector3 halfExtents;
            Quaternion orientation;

            if (GeometryTools.ThreePointsToBox(topPos, bottomPos, (prevBottomPos + prevTopPos) * 0.5f, out vector, out halfExtents, out orientation))
            {
                int num = Physics.OverlapBoxNonAlloc(vector, halfExtents, this._colliders, orientation, LayerMasks.EnemyLayerMask);
                if (num > 0)
                {
                    EvalCut(_colliders[0], vector, orientation);
                }

            }
        }
        

        private void EvalCut(Collider col, Vector3 cutPoint, Quaternion orientation)
        {
            BladeInteractableObject enemy = col.gameObject.GetComponentInParent<BladeInteractableObject>();

            if (enemy != null)
            {
                if (EvalSkewer(enemy))
                {
                    SetSkewed(enemy);
                }
                else if (EvalSlice(enemy))
                {
                    SetSliced(enemy, cutPoint, orientation);
                }
            }
        }
        
        private void SetSkewed(BladeInteractableObject enemyBase)
        {
            if (!enemyBase.IsSkewed)
                return;

            OnSkewed.Dispatch(enemyBase);
        }

        private void SetSliced(BladeInteractableObject enemyBase, Vector3 contactPoint, Quaternion orientation)
        {
            if (!enemyBase.IsSlicable)
            {
                return;
            }

            OnSliced.Dispatch(enemyBase, contactPoint, orientation, _currentData.topPos - LastData.topPos);
        }

        private bool EvalSlice(BladeInteractableObject sliceObject)
        {
            return true;
        }

        private bool EvalSkewer(BladeInteractableObject sliceObject)
        {
            if (sliceObject.IsSkewed)
                return false;

            if (sliceObject.IsCollectable)
            {
                var collectPoint = Vector3.Lerp(LastData.botPos, LastData.topPos, PokeEndPointPercentage);
                if (Vector3.Dot((sliceObject.transform.position - collectPoint).normalized, this.transform.forward) < PokeAngle)
                    return false;
            }
            else if (sliceObject.IsBomb)
            {
                // position on blade
                var collectPoint = Vector3.Lerp(LastData.botPos, LastData.topPos, BombPokeEndPointPercentage);
                if (Vector3.Dot((sliceObject.transform.position - collectPoint).normalized, this.transform.forward) < BombPokeAngle)
                    return false;
            }
            else
            {
                if (Vector3.Dot((sliceObject.transform.position - LastData.topPos).normalized, this.transform.forward) < PokeAngle)
                    return false;
            }
            
            return true;
        }



    }
}