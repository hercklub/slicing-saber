using UnityEngine;

namespace Obstacle
{
    public class ObstacleDataModel
    {
        public Vector3 StartPos;
        public Vector3 MidPos;
        public Vector3 EndPos;
        public float Time;
        public float StartTime;
        public Vector3 Scale;
        public float Rotation;

        public Vector3 PivotOffset;
        public bool IsPortal;

        public ObstacleDataModel(Vector3 startPos, Vector3 midPos, Vector3 endPos, float time, float startTime, Vector3 scale, float rotation, Vector3 pivotOffset, bool isPortal)
        {
            StartPos = startPos;
            MidPos = midPos;
            EndPos = endPos;
            Time = time;
            StartTime = startTime;
            Scale = scale;
            Rotation = rotation;
            PivotOffset = pivotOffset;
            IsPortal = isPortal;
        }
    }
}