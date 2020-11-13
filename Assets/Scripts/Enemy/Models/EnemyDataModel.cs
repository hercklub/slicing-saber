using UnityEngine;

namespace Enemy.Models
{
    public class EnemyDataModel
    {
        public int Id;
        public Vector3 StartPos;
        public Vector3 StartDir;
        public float StartForce;
        
        public EnemyDataModel(int id, Vector3 startPos, Vector3 startDir, float startForce)
        {
            Id = id;
            StartPos = startPos;
            StartDir = startDir;
            StartForce = startForce;
        }
    }
}