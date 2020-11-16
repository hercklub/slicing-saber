using System;
using UnityEngine;

public class LayerMasks
{
    protected static LayerMask _enemyLayerMask = 0;

    protected static LayerMask _domeLayerMask = 0;

    protected static LayerMask _groundLayerMask = default(LayerMask);
    
    protected static int _enemyLayer = -1;
    protected static int _fishLayer = -1;
    protected static int _grassLayer = -1;
    protected static int _obstacleLayer = -1;
    protected static int _targetLayer = -1;
    
    protected static int _visibleDimension = -1;
    protected static int _invisibleDimension = -1;


    public static LayerMask EnemyLayerMask
    {
        get
        {
            if (_enemyLayerMask.value == 0)
            {
                _enemyLayerMask = GetLayerMask("Enemy");
            }

            return _enemyLayerMask;
        }
    }

    public static int EnemyLayer
    {
        get
        {
            if (_enemyLayer == -1)
            {
                _enemyLayer = LayerMask.NameToLayer("Enemy");
            }
            return _enemyLayer;
        }
    }
    
    public static int FishLayer
    {
        get
        {
            if (_fishLayer == -1)
            {
                _fishLayer = LayerMask.NameToLayer("Fish");
            }
            return _fishLayer;
        }
    }
    
    public static int GrassLayer
    {
        get
        {
            if (_grassLayer == -1)
            {
                _grassLayer = LayerMask.NameToLayer("Grass");
            }
            return _grassLayer;
        }
    }
    
    public static int ObstacleLayer
    {
        get
        {
            if (_obstacleLayer == -1)
            {
                _obstacleLayer = LayerMask.NameToLayer("Obstacle");
            }
            return _obstacleLayer;
        }
    }
    public static int TargetLayer
    {
        get
        {
            if (_targetLayer == -1)
            {
                _targetLayer = LayerMask.NameToLayer("Target");
            }
            return _targetLayer;
        }
    }

    public static int GroundLayer
    {
        get
        {
            if (_groundLayerMask == 0)
            {
                _groundLayerMask = LayerMask.NameToLayer("Floor");
            }
            return LayerMasks._groundLayerMask;
        }
    }

    
    public static LayerMask Dome
    {
        get
        {
            if (_domeLayerMask.value == 0)
            {
                _domeLayerMask = LayerMasks.GetLayerMask("Dome");
            }
            return _domeLayerMask;
        }
    }
    
    public static int VisibleDimension
    {
        get
        {
            if (_visibleDimension == -1)
            {
                _visibleDimension = LayerMask.NameToLayer("VisibleDimension");
            }
            return _visibleDimension;
        }
    }
    
    
    public static int InVisibleDimension
    {
        get
        {
            if (_invisibleDimension == -1)
            {
                _invisibleDimension = LayerMask.NameToLayer("InvisibleDimension");
            }
            return _invisibleDimension;
        }
    }

    private static LayerMask GetLayerMask(string layerName)
    {
        int num = LayerMask.NameToLayer(layerName);
        LayerMask result = default;
        result.value = 1 << num;
        return result;
    }
}