using System;
using Enemy;
using Enemy.Models;
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
        [Inject] public IEnemyDataModels EnemyDataModels { get; set; }
        [Inject] public EnemyRemovedSignal EnemyRemovedSignal { get; set; }

        
        public int Id;
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
            EnemyRemovedSignal.RemoveListener(EnemyRemovedHandler);
        }

        
        void Awake()
        {
            gameObject.SetActive(false);
        }
        
        private void Init()
        {
            gameObject.SetActive(true);
            EnemyRemovedSignal.AddListener(EnemyRemovedHandler);
        }
        

        public void Deploy()
        {
            Init();
            
            EnemyDataModel enemyData = EnemyDataModels.GetTarget(Id);
            transform.position = enemyData.StartPos;
            
            Rb.velocity = Vector3.zero;
            Rb.AddForce(enemyData.StartDir * enemyData.StartForce, ForceMode.Impulse);
            
        }

        public void SetSliced(Vector3 contactPoint, ControlerHand hand, Quaternion orientation, Vector3 cutDir)
        {
            DestroyEnemy(true);
        }

        private void DestroyEnemy(bool success)
        {
            EnemyDataModels.AddScore(success);
            EnemyDataModels.RemoveEnemy(Id);
        }
        

        private void Update()
        {
            if (transform.position.y < -2f)
            {
                DestroyEnemy(false);
            }
        }
        
        private void EnemyRemovedHandler(int id)
        {
            if (Id == id)
            {
                DestroyEnemyObject();
            }
        }
        
        private void DestroyEnemyObject()
        {
            EnemyObjectsPool.ReturnInstance(this);
        }

    }
}