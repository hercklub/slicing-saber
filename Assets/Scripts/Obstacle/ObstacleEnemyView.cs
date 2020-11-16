using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Serialization;

namespace Obstacle
{
    public class ObstacleEnemyView : View
    {
        public ObstacleDataModel ObstacleData;
        public Transform PlaneTranform;
        public Transform PlanePivot;
        public GameObject WindowMask;
        public AudioClip MissSounds;
        public AudioSource CollectSource;
        public MeshRenderer Renderer;

        public Bounds Bounds;
        
        public Material NormalObstacleMat;
        public Material PortalObstacleMat;
        public Color ObstacleColor;
        public float ProgressAlongPath;

        
        public void PlayMissSound()
        {
            CollectSource.transform.SetParent(null,true);
            CollectSource.clip = MissSounds;
            CollectSource.volume = 1f;
            CollectSource.Play();
        }
    }
}