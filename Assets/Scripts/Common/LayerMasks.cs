using System;
using UnityEngine;

public class LayerMasks
{
    protected static LayerMask _enemyLayerMask = 0;
    protected static LayerMask _boundaryLayerMask = 0;

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

    public static LayerMask BoundaryLayerMask
    {
        get
        {
            if (_boundaryLayerMask.value == 0)
            {
                _boundaryLayerMask = GetLayerMask("Boundary");
            }

            return _boundaryLayerMask;
        }
    }


    private static LayerMask GetLayerMask(string layerName)
    {
        int num = LayerMask.NameToLayer(layerName);
        LayerMask result = default(LayerMask);
        result.value = 1 << num;
        return result;
    }
}