using System;
using UnityEngine;

public class LayerMasks
{
    protected static LayerMask _enemyLayerMask = 0;

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

    

    private static LayerMask GetLayerMask(string layerName)
    {
        int num = LayerMask.NameToLayer(layerName);
        LayerMask result = default;
        result.value = 1 << num;
        return result;
    }
}