using Blade;
using Level;
using PathCreation;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor
{
    public class EventObstacleObjectView : View, IPoolable
    {
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        
        public GameObject Obstacle;
        public float Radius { get; set; }
        public bool retain { get; set; }

        public Renderer ObstacleRenderer;
        public Material NormalMaterial;
        public Material PortalMaterial;

        
        public LineRenderer PathLineRenderer;
        private EditorObstacleData _data;
        private Bounds _bounds = new Bounds(Vector3.zero, Vector3.one * 2f);
        private VertexPath _vertexPath;
        
        public void Restore()
        {
            EventObjectSelectedSignal.RemoveListener(EventObjectSelectedHandler);
            LevelEditorControler.LineModeChanched.RemoveListener(LinesModeChangedHandler);
            LevelEditorControler.ObstaclesVisible.RemoveListener(ObstacleVisibleHanadler);

            gameObject.SetActive(false);
        }

        // called when instance is being used
        public void Deploy()
        {
            gameObject.SetActive(true);
            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            LevelEditorControler.LineModeChanched.AddListener(LinesModeChangedHandler);
            LevelEditorControler.ObstaclesVisible.AddListener(ObstacleVisibleHanadler);
        }

        private void ObstacleVisibleHanadler(bool visible)
        {
            Obstacle.gameObject.SetActive(visible);
        }
        public void Retain()
        {
            retain = true;
        }

        public void Release()
        {
            retain = false;
        }

        public EditorObstacleData Data
        {
            get => _data;
        }
        
        public void SetData(EditorObstacleData data, float radius)
        {
            _data = data;
            Radius = radius;
            Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);

            if (data.IsPortal)
            {
                ObstacleRenderer.material = PortalMaterial;
            }
            else
            {
                ObstacleRenderer.material = NormalMaterial;
            }
            var progressInArena = radius / 20f;
            if (progressInArena <= 1f)
            {
                _vertexPath = EditorHelpers.GetObstacleObjectPath(data, _bounds);

                if (LevelEditorControler.AllLinesMode)
                {
                    SetPathLine(true);
                }

                transform.position = _vertexPath.GetPointAtDistance(progressInArena * _vertexPath.length,
                    EndOfPathInstruction.Stop);

                var rot = _vertexPath.GetRotationAtDistance(progressInArena * _vertexPath.length);
                transform.localRotation = rot;
            }
            else
            {
                SetPathLine(false);
                pos.y = data.StartHeight;
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
            }
            
            transform.localScale = new Vector3(data.ScaleX, data.ScaleY, 1f);
            Obstacle.transform.localPosition = data.Pivot.ToPivot() / 2f;
        }
        
        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type == EnemyType.Obstacle )
            {
                if (index == _data.Index)
                {
                    if (!LevelEditorControler.AllLinesMode)
                    {
                        SetPathLine(true);
                    }
                }
                else
                {
                    if (!LevelEditorControler.AllLinesMode)
                    {
                        SetPathLine(false);
                    }
                }

            }
        }
        private void LinesModeChangedHandler()
        {
            SetPathLine(LevelEditorControler.AllLinesMode);
        }
        
        private void SetPathLine(bool isEnabled)
        {
            if (isEnabled)
            {
                PathLineRenderer.gameObject.SetActive(true);
                PathLineRenderer.positionCount = _vertexPath.vertices.Length;
                PathLineRenderer.SetPositions(_vertexPath.vertices);
            }
            else
            {
                PathLineRenderer.gameObject.SetActive(false);
            }
        }
    }
}