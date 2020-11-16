using System;
using Framewerk.UI;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor
{
    public class AddModePopupMediator : ExtendedMediator
    {
        [Inject] public AddModePopupView View { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }

        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        
        private EventEditType _editType = EventEditType.None;
        
        public override void OnRegister()
        {
            base.OnRegister();
            
            AddButtonListener(View.Normal, NormalToggleHandler);
            AddButtonListener(View.Collectable, CollectableHandler);
            AddButtonListener(View.Bullet, BulletHandler);
            AddButtonListener(View.Target, TargetHandler);
            AddButtonListener(View.Obstacle, ObstacleHandler);
            AddButtonListener(View.SideBomb, FishHandler);
            AddButtonListener(View.Grass, GrassHandler);
            
            
            EditModeChangedSignal.AddListener(EditModeChangedHandler);
            SetControlMode(EventEditType.Red);
            View.SetEnabled(false);
        }
        
        private void NormalToggleHandler()
        {
            SetControlMode(EventEditType.Red);
        }
        private void CollectableHandler()
        {
            SetControlMode(EventEditType.Yellow);
        }
        
        private void BulletHandler()
        {
            SetControlMode(EventEditType.Bullet);
        }
        
        private void TargetHandler()
        {
            SetControlMode(EventEditType.Target);
        }
        private void GrassHandler()
        {
            SetControlMode(EventEditType.Grass);
        }

        private void FishHandler()
        {
            SetControlMode(EventEditType.SideBomb);
        }
        
        private void ObstacleHandler()
        {
            SetControlMode(EventEditType.Obstacle);
        }
        
        private void SetControlMode(EventEditType type)
        {
            _editType = type;
            ResetAll();
            switch (type)
            {
                case EventEditType.None:
                    break;
                case EventEditType.Red:
                    View.Normal.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.Yellow:
                    View.Collectable.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.Bullet:
                    View.Bullet.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.Target:
                    View.Target.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.SideBomb:
                    View.SideBomb.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.Obstacle:
                    View.Obstacle.image.color = View.ToggleOnColor;
                    break;
                case EventEditType.Grass:
                    View.Grass.image.color = View.ToggleOnColor;
                    break;
            }
            LevelEditorControler.CurrentEventType = _editType;
        }
        
        private void EditModeChangedHandler(EditorMode type)
        {
            View.SetEnabled(type == EditorMode.Add);
        }

        private void ResetAll()
        {
            View.Normal.image.color = View.ToggleOffColor;
            View.Collectable.image.color = View.ToggleOffColor;
            View.Bullet.image.color = View.ToggleOffColor;
            View.Target.image.color = View.ToggleOffColor;
            View.Obstacle.image.color = View.ToggleOffColor;
            View.SideBomb.image.color = View.ToggleOffColor;
            View.Grass.image.color = View.ToggleOffColor;
  
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetControlMode(EventEditType.Red);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetControlMode(EventEditType.Yellow);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetControlMode(EventEditType.Bullet);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetControlMode(EventEditType.Target);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetControlMode(EventEditType.SideBomb);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetControlMode(EventEditType.Obstacle);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetControlMode(EventEditType.Grass);
            }
        }
    }
}