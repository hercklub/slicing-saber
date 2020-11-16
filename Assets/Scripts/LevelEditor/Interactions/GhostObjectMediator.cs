using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor.Interactions
{
    public class GhostObjectMediator : Mediator
    {
        [Inject] public GhostObjectUpdateSignal GhostObjectUpdateSignal { get; set; }
        [Inject] public GhostObjectView View { get; set; }

        private EventEditType _currentType;

        public override void OnRegister()
        {
            base.OnRegister();
            GhostObjectUpdateSignal.AddListener(GhostObjectUpdateHandler);
        }

        public override void OnRemove()
        {
            GhostObjectUpdateSignal.RemoveListener(GhostObjectUpdateHandler);
            base.OnRemove();
        }

        private void GhostObjectUpdateHandler(Vector3 pos, EventEditType type)
        {
            if (type != _currentType)
                EnableType(type);
            
            View.transform.position = pos;
            if (Mathf.Approximately(pos.x, 0f) && Mathf.Approximately(pos.z, 0f))
                return;
            View.transform.rotation = Quaternion.LookRotation(new Vector3(pos.x,0f, pos.z), Vector3.up);
            _currentType = type;
        }

        private void EnableType(EventEditType type)
        {
            ResetAll();
            switch (type)
            {
                case EventEditType.None:
                    break;
                case EventEditType.Red:
                    View.Red.SetActive(true);
                    break;
                case EventEditType.Yellow:
                    View.Yellow.SetActive(true);
                    break;
                case EventEditType.Bullet:
                    View.Bullet.SetActive(true);
                    break;
                case EventEditType.Target:
                    View.Target.SetActive(true);
                    break;
                case EventEditType.SideBomb:
                    View.Fish.SetActive(true);
                    break;
                case EventEditType.Obstacle:
                    View.Obstacle.SetActive(true);
                    break;
                case EventEditType.Grass:
                    View.Grass.SetActive(true);
                    break;

            }
        }
        private void ResetAll()
        {
            View.Red.SetActive(false);
            View.Yellow.SetActive(false);
            View.Bullet.SetActive(false);
            View.Target.SetActive(false);
            View.Fish.SetActive(false);
            View.Obstacle.SetActive(false);
            View.Grass.SetActive(false);
        }
    }
}