using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor
{
    public class EventGrassObjectView :  View, IPoolable
    {
        public GameObject Grass;
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
        
        public EditorGrassData Data
        {
            get => _data;
        }
        
        private EditorGrassData _data;
        
        public void SetData(EditorGrassData data, float radius)
        {
            _data = data;
            Radius = radius;
            Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(new Vector3(pos.x,0f, pos.z), Vector3.up);
        }
    }
}