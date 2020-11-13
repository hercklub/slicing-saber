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

        public float BladeSpeed;

        private Collider[] _colliders = new Collider[16];
        
        public Signal<BladeInteractableView, Vector3, Quaternion, Vector3> OnSliced =
            new Signal<BladeInteractableView, Vector3, Quaternion, Vector3>();

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
                for (int i = 0; i < num; i++)
                {
                    EvalCut(_colliders[i], vector, orientation);
                }
            }
        }
        

        private void EvalCut(Collider col, Vector3 cutPoint, Quaternion orientation)
        {
            BladeInteractableView enemy = col.gameObject.GetComponentInParent<BladeInteractableView>();

            if (enemy != null)
            {
                if (EvalSlice(enemy))
                {
                    SetSliced(enemy, cutPoint, orientation);
                }
            }
        }
        

        private void SetSliced(BladeInteractableView enemyBase, Vector3 contactPoint, Quaternion orientation)
        {
            if (!enemyBase.IsSlicable)
            {
                return;
            }

            OnSliced.Dispatch(enemyBase, contactPoint, orientation, _currentData.topPos - LastData.topPos);
        }

        private bool EvalSlice(BladeInteractableView sliceObject)
        {
            return true;
        }
        

    }
}