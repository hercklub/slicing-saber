using System;
using Blade;
using Level;
using LevelEditor.EventsList;
using PathCreation;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor
{
    public class EventEnemyObjectView : View, IPoolable
    {
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }


        public GameObject RedBox;
        public GameObject YellowBox;
        public GameObject Bullet;
        public GameObject Direction;


        public Renderer LeftCutDir;
        public Renderer RightCutDir;
        public Renderer DownCutDir;
        public Renderer UpCutDir;

        public Material EnabledMaterial;
        public Material DisabledMaterial;

        public LineRenderer PathLineRenderer;
        private EditorEnemyData _data;


        private Bounds _bounds = new Bounds(Vector3.zero, Vector3.one * 2f);

        private VertexPath _vertexPath;

        public EditorEnemyData Data
        {
            get => _data;
        }

        public float Radius { get; set; }

        public bool retain { get; set; }

        public void Restore()
        {
            gameObject.SetActive(false);
            EventObjectSelectedSignal.RemoveListener(EventObjectSelectedHandler);
            LevelEditorControler.LineModeChanched.RemoveListener(LinesModeChangedHandler);
        }

        // called when instance is being used
        public void Deploy()
        {
            gameObject.SetActive(true);
            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            LevelEditorControler.LineModeChanched.AddListener(LinesModeChangedHandler);
        }


        public void Retain()
        {
            retain = true;
        }

        public void Release()
        {
            retain = false;
        }

        public void SetData(EditorEnemyData data, float radius)
        {
            _data = data;
            Radius = radius;
            Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);

            var progressInArena = radius / 20f;
            if (progressInArena <= 1f)
            {
                _vertexPath = EditorHelpers.GetEnemyObjectPath(data, _bounds);

                if (LevelEditorControler.AllLinesMode)
                {
                    SetPathLine(true);
                }

                transform.position = _vertexPath.GetPointAtDistance(progressInArena * _vertexPath.length,
                    EndOfPathInstruction.Stop);

                Vector3 rotationAxis = data.EndRotation.ToVector();
                float sideRotation = data.EndRotation != EnemyRotation.None ? 45f : 0f;
                var rot = _vertexPath.GetRotationAtDistance(progressInArena * _vertexPath.length);

                transform.localRotation = rot * Quaternion.Lerp(Quaternion.AngleAxis(sideRotation, rotationAxis),Quaternion.identity, progressInArena);
            }
            else
            {
                SetPathLine(false);
                pos.y = data.StartHeight;
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
            }

            if (data.Type == EnemyAirType.Normal)
            {
                SetCutDir(data.CutDir);
                EnableCutDir(true);
            }
            else
            {
                EnableCutDir(false);
            }

            Direction.transform.localPosition = data.EndDirection.ToVector(_bounds) / 4f;
            Direction.transform.localRotation = Quaternion.LookRotation(data.EndDirection.ToVector(_bounds), transform.up);


            EnableType(data.Type);
        }


        private void EnableCutDir(bool isEnabled)
        {
            LeftCutDir.gameObject.SetActive(isEnabled);
            RightCutDir.gameObject.SetActive(isEnabled);
            DownCutDir.gameObject.SetActive(isEnabled);
            UpCutDir.gameObject.SetActive(isEnabled);
        }

        private void SetCutDir(eCutDir cutDir)
        {
            LeftCutDir.material = cutDir.HasAxisFlag(eCutDir.Left) ? EnabledMaterial : DisabledMaterial;
            RightCutDir.material = cutDir.HasAxisFlag(eCutDir.Right) ? EnabledMaterial : DisabledMaterial;
            DownCutDir.material = cutDir.HasAxisFlag(eCutDir.Down) ? EnabledMaterial : DisabledMaterial;
            UpCutDir.material = cutDir.HasAxisFlag(eCutDir.Up) ? EnabledMaterial : DisabledMaterial;
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

        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type == EnemyType.Air )
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

        private void EnableType(EnemyAirType type)
        {
            DisableAllTypes();
            switch (type)
            {
                case EnemyAirType.None:
                    break;
                case EnemyAirType.Normal:
                    RedBox.SetActive(true);
                    break;
                case EnemyAirType.Collectable:
                    YellowBox.SetActive(true);
                    break;
                case EnemyAirType.Bullet:
                    Bullet.SetActive(true);
                    break;
            }
        }

        private void DisableAllTypes()
        {
            RedBox.SetActive(false);
            YellowBox.SetActive(false);
            Bullet.SetActive(false);
        }
    }
}