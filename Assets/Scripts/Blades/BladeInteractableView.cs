using System;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Blades
{
    public interface IBladeInteractable
    {
        void SetSliced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir);

    }

    public class BladeInteractableView : View, IBladeInteractable, IPoolable
    {
        [Inject] public IPool<BladeInteractableView> EnemyObjectsPool { get; set; }

        public bool IsSlicable;
        public Rigidbody Rb;

        public bool retain { get; set; }
        
        public void Retain()
        {
            retain = true;
        }

        public void Release()
        {
            retain = false;
        }

        public void Restore()
        {
            gameObject.SetActive(false);
            Rb.velocity = Vector3.zero;

        }

        public void Deploy()
        {
            transform.position = Vector3.zero;

            gameObject.SetActive(true);
            
        }

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetSliced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir)
        {
            //EnemyObjectsPool.ReturnInstance(this);
        }

        public void DestroyObject()
        {
            EnemyObjectsPool.ReturnInstance(this);
        }

        private void Update()
        {
            if (transform.position.y < -5f)
            {
                DestroyObject();
            }
        }
    }
}