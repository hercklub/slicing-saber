using UnityEngine;

public class GeometryTools
{
    public static bool ThreePointsToBox(Vector3 p0, Vector3 p1, Vector3 p2, out Vector3 center, out Vector3 halfSize, out Quaternion orientation)
    {
        Vector3 normalized = Vector3.Cross(p1 - p2, p0 - p2).normalized;
        if (normalized.sqrMagnitude > 1E-05f)
        {
            Vector3 normalized2 = (p0 - p1).normalized;
            Vector3 vector = Vector3.Cross(normalized2, normalized);
            orientation = default(Quaternion);
            orientation.SetLookRotation(normalized2, normalized);
            float num = Mathf.Abs(new Plane(vector, p0).GetDistanceToPoint(p2));
            float num2 = Vector3.Magnitude(p0 - p1);
            Vector3 a = (p0 + p1) * 0.5f;
            center = a - vector * num * 0.5f;
            halfSize = new Vector3(num * 0.5f, 0f, num2 * 0.5f);
            return true;
        }
        center = Vector3.zero;
        halfSize = Vector3.zero;
        orientation = Quaternion.identity;
        return false;
    }
}