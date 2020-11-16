using Framewerk.UI.List;
using TMPro;
using UnityEngine;

namespace LevelEditor.EventsList
{
    public class EventItemView : ListItemView
    {
        public TMP_Text LevelLabel;
        public TMP_Text TimeLabel;
        public LineRenderer Circle;
        public int CircleSegments = 360;
        
        public  void DrawCircle( float radius, float lineWidth, Color color)
        {
            Circle.startWidth = lineWidth;
            Circle.endWidth = lineWidth;
            Circle.positionCount = CircleSegments + 1;
            Circle.startColor = color;
            Circle.endColor = color;
            var pointCount = CircleSegments + 1; // add extra point to make startpoint and endpoint the same to close the circle
            var points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / CircleSegments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            Circle.SetPositions(points);
        }
    }
}