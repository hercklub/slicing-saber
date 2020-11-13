using UnityEngine;

public enum ControlerHand
{
    None = 0,
    Left ,
    Right
}

public struct BladeData
{
    public BladeData(Vector3 botPos, Vector3 topPos, Quaternion rot, Vector3 forward)
    {
        this.botPos = botPos;
        this.topPos = topPos;
        this.rot = rot;
        this.forward = forward;
    }

    public Vector3 botPos;
    public Vector3 topPos;
    public Quaternion rot;
    public Vector3 forward;
}

