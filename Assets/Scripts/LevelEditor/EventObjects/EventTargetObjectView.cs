using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor
{
    public enum TargetType
    {
        Normal = 0,
    }

    
    public class EventTargetObjectView : View, IPoolable
    {  
        public GameObject Target;
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
        
        public EditorTargetData Data
        {
            get => _data;
        }
        
        private EditorTargetData _data;

        public void SetData(EditorTargetData data, float radius)
        {
            _data = data;
            Radius = radius;
            Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius + data.Depth);
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(new Vector3(pos.x,0f, pos.z), Vector3.up);
            EnableType(data.TargetType);
        }

        private void EnableType(TargetType type)
        {
            DisableAllTypes();
            switch (type)
            {
                case TargetType.Normal:
                    Target.SetActive(true);
                    break;
            }
        }

        private void DisableAllTypes()
        {
            Target.SetActive(false);

        }
    }
}