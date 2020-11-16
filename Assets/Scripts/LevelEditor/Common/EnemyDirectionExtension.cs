using System;
using UnityEngine;

namespace Blade
{

    public static class EnemyDirectionExtension
    {
   
        public static Vector3 ToVector(this EnemyDirection cutDirection, Bounds bounds)
        {
            Vector3 direction = Vector3.zero;
            int dirIndex = (int) cutDirection;
            float horizontalStep = bounds.size.x / 4f;
            float verticalStep = bounds.size.y / 4f;

            int row = dirIndex / 4;
            int col = dirIndex % 4;
            
            
            // calculate pos, offset to minus values
            float x = (col * horizontalStep) - horizontalStep * (4 / 2f) + (horizontalStep / 2f) ;
            float y = (row * verticalStep) - verticalStep * (4 / 2f) + (verticalStep /2f);
            
            direction = new Vector3(x, y, 0f );
            return direction;

        }
        
        public static Vector3 ToVector(this ObstacleEndDir cutDirection, Bounds bounds)
        {
            Vector3 direction = Vector3.zero;
            int dirIndex = (int) cutDirection;
            float horizontalStep = bounds.size.x / 2f;
            float verticalStep = bounds.size.y / 2f;

            int row = dirIndex / 3;
            int col = dirIndex % 3;
            
            
            // calculate pos, offset to minus values
            float x = (col * horizontalStep) - horizontalStep ;
            float y = (row * verticalStep) - verticalStep;
            
            direction = new Vector3(x, y, 0f );
            return direction;

        }
        
        public static Vector3 ToPivot(this ObstacleEndDir cutDirection)
        {
            Vector3 direction = Vector3.zero;
            switch (cutDirection)
            {
                case ObstacleEndDir.DownLeft:
                    return Vector3.down + Vector3.left;
                case ObstacleEndDir.Down:
                    return Vector3.down;
                case ObstacleEndDir.DownRight:
                    return Vector3.down + Vector3.right;
                case ObstacleEndDir.Left:
                    return Vector3.left;
                case ObstacleEndDir.Mid:
                    return Vector3.zero;
                case ObstacleEndDir.Right:
                    return Vector3.right;
                case ObstacleEndDir.UpLeft:
                    return Vector3.up + Vector3.left;
                case ObstacleEndDir.Up:
                    return Vector3.up;
                case ObstacleEndDir.UpRight:
                    return Vector3.up + Vector3.right;
            }
            return direction;

        }
        
        public static Vector3 ToVector(this EnemyRotation enemyRotation)
        {
            Vector3 direction = Vector3.zero;
            switch (enemyRotation)
            {
                case EnemyRotation.None:
                    break;
                case EnemyRotation.Forward:
                    direction = Vector3.forward;
                    break;
                case EnemyRotation.Up:
                    direction = Vector3.up;
                    break;
                case EnemyRotation.Right:
                    direction = Vector3.right;
                    break;
                case EnemyRotation.BackWard:
                    direction = -Vector3.forward;
                    break;
                case EnemyRotation.Down:
                    direction = -Vector3.up;
                    break;
                case EnemyRotation.Left:
                    direction = -Vector3.right;
                    break;

            }

            return direction;
        }
    }
}