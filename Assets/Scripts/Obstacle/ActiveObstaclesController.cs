using System;
using System.Collections.Generic;
using Common;
using Framewerk;
using Framewerk.StrangeCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacle
{
    public interface IActiveObstaclesController
    {
        void Init();
        void Destroy();
        void AddObstacle(ObstacleEnemyView obstacle);
        void RemoveObstacle(ObstacleEnemyView obstacle);
        List<ObstacleEnemyView> ActiveObstacleControllers { get; }
    }

    public class ActiveObstaclesController : IActiveObstaclesController
    {

        [Inject] public IUpdater Updater { get; set; }
        private List<ObstacleEnemyView> _activeObstacleControllers = new List<ObstacleEnemyView>();

        private MaterialPropertyBlock _materialPropertyBlock;
        protected int _colorID;
        public List<ObstacleEnemyView> ActiveObstacleControllers
        {
            get
            {
                return this._activeObstacleControllers;
            }
        }

        public void Init()
        {
            Destroy();
            Updater.EveryFrame(ObstacleUpdate);
            _materialPropertyBlock = new MaterialPropertyBlock();
            _colorID = Shader.PropertyToID("_Color");
        }

        private void ObstacleUpdate()
        {
            for (int i = 0; i < _activeObstacleControllers.Count; i++)
            {
                var obstacle = _activeObstacleControllers[i];
                obstacle.ObstacleColor.S(Mathf.Lerp(0.2f, 1f ,obstacle.ProgressAlongPath), ref obstacle.ObstacleColor);
                _materialPropertyBlock.SetColor(_colorID, obstacle.ObstacleColor);

                    obstacle.Renderer.SetPropertyBlock(_materialPropertyBlock);

            }
        }

        public void Destroy()
        {
            Updater.RemoveFrameAction(ObstacleUpdate);
        }
        

        public void AddObstacle(ObstacleEnemyView obstacle)
        {
            _activeObstacleControllers.Add(obstacle);
        }

        public void RemoveObstacle(ObstacleEnemyView obstacle)
        {
            _activeObstacleControllers.Remove(obstacle);
        }
    }
}