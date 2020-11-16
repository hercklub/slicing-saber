using System;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    public enum BombType
    {
        Water = 0,
        Good
    }
    
    public class EventFishObjectView :  View, IPoolable
    {
        public GameObject Bomb;
       public GameObject BombGood;
        public float Radius { get; set; }
        
        public bool retain { get; set; }
        
        public void Restore()
        {
            gameObject.SetActive(false);
        }

        // called when instance is being used
        public void Deploy()
        {
            gameObject.SetActive(true); 
        }

        public void Retain()
        {
            retain = true;
        }

        public void Release()
        {
            retain = false;
        }
        
        public EditorFishData Data
        {
            get => _data;
        }
        
        private EditorFishData _data;

        public void SetData(EditorFishData data, float radius)
        {
            _data = data;
            Radius = radius;
            Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(new Vector3(pos.x,0f, pos.z), Vector3.up);
            EnableType(data.FishType);
        }
        
        private void EnableType(BombType type)
        {
            DisableAllTypes();
            switch (type)
            {
                case BombType.Water:
                    Bomb.SetActive(true);
                    break;
                case  BombType.Good:
                    BombGood.SetActive(true);
                    break;
            }
        }

        private void DisableAllTypes()
        {
            Bomb.SetActive(false);
            BombGood.SetActive(false);
        }

    }
}