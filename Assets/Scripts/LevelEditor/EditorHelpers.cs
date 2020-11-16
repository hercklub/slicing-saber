using Blade;
using PathCreation;
using UnityEngine;

namespace LevelEditor
{
    public static class EditorHelpers
    {
        public static Vector3 GetPointOnCircle(float angle, float radius)
        {
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius,0f ,Mathf.Cos(angle *  Mathf.Deg2Rad) * radius);
        }
        public static Vector3 GetPointOnCircle(float x, float y, float radius)
        {

            return new Vector3(
                Mathf.Sin(x * Mathf.Deg2Rad) * Mathf.Cos(y * Mathf.Deg2Rad) * radius,
                Mathf.Sin(y * Mathf.Deg2Rad) * radius,
                Mathf.Cos(x * Mathf.Deg2Rad) * Mathf.Cos(y * Mathf.Deg2Rad) * radius); 
                ;
            
         //   x = radius * sin(yaw) * cos(pitch), y = radius * sin(pitch), z = radius * cos(yaw) * cos(pitch)
        }
        
        
        public static VertexPath GetEnemyObjectPath(EditorEnemyData data, Bounds headBounds)
        {
            Vector3 startPos = GetPointOnCircle(data.StartPos * 360f, data.StartHeight * 360f, 20f);
           //startPos.y = Mathf.Sin(data.StartHeight / 20f * 360f * Mathf.Deg2Rad);

           Vector3 midPos = GetPointOnCircle(data.MidPos * 360f, 10f);
            midPos.y = data.MidHeight;
            
      
            Vector3 endPosDir = data.EndDirection.ToVector(headBounds);
            Vector3 enemyDir = startPos.normalized;
            var lookRotation = Quaternion.LookRotation(enemyDir, Vector3.up);
            endPosDir = lookRotation * endPosDir;
            Vector3 endPos = Vector3.up * 1.4f +  endPosDir;
             
            
            return GeneratePath(startPos, midPos, endPos);
        }

        public static VertexPath GetObstacleObjectPath(EditorObstacleData data, Bounds headBounds)
        {
            Vector3 startPos = GetPointOnCircle(data.StartPos * 360f, data.StartHeight * 360f, 20f);
            //startPos.y = Mathf.Sin(data.StartHeight / 20f * 360f * Mathf.Deg2Rad);

            Vector3 midPos = GetPointOnCircle(data.MidPos * 360f, 10f);
            midPos.y = data.MidHeight;
            
      
            Vector3 endPosDir = data.EndDirection.ToVector(headBounds);
            Vector3 enemyDir = startPos.normalized;
            var lookRotation = Quaternion.LookRotation(enemyDir, Vector3.up);
            endPosDir = lookRotation * endPosDir;
            Vector3 endPos = Vector3.up * 1.4f +  endPosDir;
             
            
            return GeneratePath(startPos, midPos, endPos);
        }
        private static VertexPath GeneratePath(Vector3 startPoint, Vector3 midPoint, Vector3 endPoint)
        {
            Vector3 startTempPoint = startPoint * 0.8f;
            startTempPoint.y = startPoint.y;
            Vector3 endTempPoint = endPoint * 2f;
            endTempPoint.y = endPoint.y;
            
            Vector3[] points = new[] {endPoint, midPoint , startPoint};
            BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            bezierPath.ResetNormalAngles();
            bezierPath.GlobalNormalsAngle = 90f;
            
            return new VertexPath(bezierPath);
        }
        
        public static float WrapAngle(float input) // replace int with whatever your type is
        {
            // this will always return an angle between 0 and 360:
            // the inner % 360 restricts everything to +/- 360
            // +360 moves negative values to the positive range, and positive ones to > 360
            // the final % 360 caps everything to 0...360
            return ((input % 360) + 360) % 360;
        }

        public static float GetPointRadius(Vector3 center, Vector3 point)
        {
            return Vector3.Distance(center, point);
        }

        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static float Round(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
            //return value;
        }
    }
}