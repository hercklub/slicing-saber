using System;
using BehindYou.Scripts.Controller;
using Level;
using PathCreation;
using Plugins.Framewerk;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Obstacle
{
    public class ObstacleEnemyMediator : Mediator
    {
        [Inject] public ObstacleEnemyView View { get; set; }
        [Inject] public ILevelControler LevelControler { get; set; }
        [Inject] public IActiveObstaclesController ActiveObstaclesController { get; set; }
        
        [Inject] public PassedPortalSignal PassedPortalSignal { get; set; }
        [Inject] public ObstaclePassedSignal ObstaclePassedSignal { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }


        private ObstacleDataModel _obstacleDataModel;
        private VertexPath _vertexPath;
        
        private float _timer = 8f;
        private float _progressAlongPath;
        private bool _playerPassed = false;
        private Vector3 pivotOffset;
        
        float _levelTime = 0f;

        private Color _obstacleColor;

        public override void OnRegister()
        {
            base.OnRegister();
            _obstacleDataModel = View.ObstacleData;
            _vertexPath = GeneratePath(_obstacleDataModel.StartPos, _obstacleDataModel.MidPos,
                _obstacleDataModel.EndPos);
            
            _timer = _obstacleDataModel.Time;
            
            View.PlaneTranform.localScale = _obstacleDataModel.Scale;
            pivotOffset = _obstacleDataModel.PivotOffset;
            
            View.Bounds = new Bounds(Vector3.zero,
                new Vector3(_obstacleDataModel.Scale.x, _obstacleDataModel.Scale.y, 0.2f));

            
            View.PlanePivot.localPosition = new Vector3(pivotOffset.x * _obstacleDataModel.Scale.x,
                pivotOffset.y * _obstacleDataModel.Scale.y, 0f);
            View.WindowMask.gameObject.SetActive(_obstacleDataModel.IsPortal);
            View.WindowMask.transform.localScale = _obstacleDataModel.Scale;
            View.Renderer.material = _obstacleDataModel.IsPortal ? View.PortalObstacleMat : View.NormalObstacleMat;

            
            ActiveObstaclesController.AddObstacle(View);
        }

        public override void OnRemove()
        {
            ActiveObstaclesController.RemoveObstacle(View);

            base.OnRemove();
        }


        private void RestartGameSignal()
        {
            DestroyObstacle();
        }

        private void LevelPausedHandler(bool paused)
        {
            gameObject.SetActive(!paused);
        }

        private void LevelEndHandler(bool isCompleted)
        {
            gameObject.SetActive(false);
        }

        private void DestroyObstacle()
        {
            OnRemove();
        }
        
        private void Update()
        {
             if (_vertexPath == null)
            {
                return;
            }

            if (_progressAlongPath >= 1)
            {
                if (!_playerPassed)
                {
                    View.PlayMissSound();
                }

                ObstaclePassedSignal.Dispatch(_playerPassed);
                DestroyObstacle();

            }
            _levelTime = LevelControler.CurrentLevelTime;

            _progressAlongPath = _levelTime - _obstacleDataModel.StartTime;
            
            _progressAlongPath = _progressAlongPath / _timer;
            View.ProgressAlongPath = _progressAlongPath;

            
            transform.localPosition = _vertexPath.GetPointAtDistance(_progressAlongPath * _vertexPath.length,
                EndOfPathInstruction.Stop);

            var rot = _vertexPath.GetRotationAtDistance(_progressAlongPath * _vertexPath.length,
                EndOfPathInstruction.Stop);
            transform.localRotation = rot;
            

            Vector3 point = View.PlanePivot.InverseTransformPoint(ViewConfig.Camera3d.transform.position);
            
            if (!_playerPassed && View.Bounds.Contains(point))
            {
                _playerPassed = true;
                if (_obstacleDataModel.IsPortal)
                {
                    PassedPortalSignal.Dispatch();
                    View.WindowMask.gameObject.SetActive(false);
                }
            }
        }
        
        private VertexPath GeneratePath(Vector3 startPoint, Vector3 midPoint, Vector3 endPoint)
        {
            Vector3[] points = new[] {startPoint, midPoint, endPoint};
            BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            bezierPath.GlobalNormalsAngle = 90f;

            return new VertexPath(bezierPath);
        }
    }
}