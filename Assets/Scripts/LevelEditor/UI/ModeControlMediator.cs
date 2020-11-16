using System;
using Framewerk.UI;
using Level;
using UnityEngine;

namespace LevelEditor
{
    public enum EditorMode
    {
        None,
        Add,
        Edit,
        Save
    }

    public enum EventEditType
    {
        None,
        Red,
        Yellow,
        Bullet,
        Target,
        SideBomb,
        Obstacle,
        Grass
    }

    public class ModeControlMediator : ExtendedMediator
    {
        [Inject] public ModeControlView View { get; set; }

        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }
        
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public ShowLevelInfoDataSignal ShowLevelInfoDataSignal { get; set; }

        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }


        private EditorMode _editorMode = EditorMode.None;
        
        public override void OnRegister()
        {
            base.OnRegister();
            AddButtonListener(View.SaveButton, SaveButtonHandler);
            AddButtonListener(View.EditButton, EditButtonHandler);
            AddButtonListener(View.AddButton, AddButtonHandler);
            AddButtonListener(View.PathModeButton, PathButtonHandler);
            AddButtonListener(View.ObstaclesVisible, ObstaclesVisible);
            AddSliderListener(View.LevelSlider, LevelSliderHandler);
            
            AddButtonListener(View.GotoLayer, GotoLayerHandler);
        }




        private void AddButtonHandler()
        {
            SetControlMode(EditorMode.Add);
            EventObjectSelectedSignal.Dispatch(EnemyType.None, -1);
            ShowLevelInfoDataSignal.Dispatch( false);

        }
        private void EditButtonHandler()
        {
            SetControlMode(EditorMode.Edit);
            ShowLevelInfoDataSignal.Dispatch( false);

        }

        private void SaveButtonHandler()
        {
            SetControlMode(EditorMode.Save);
            EventObjectSelectedSignal.Dispatch(EnemyType.None, -1);
            //get current level data
            ShowLevelInfoDataSignal.Dispatch( true);
            
        }

        private void LevelSliderHandler(float progress)
        {
            //LevelEditorControler.ChangeCurrentIndex();
            int gotoLayer = Mathf.FloorToInt(1000 * progress);
            LevelEditorControler.CurrentLayerIndex = gotoLayer;
            LevelEditorControler.IndexChangedSignal.Dispatch();
        }
        

        private void GotoLayerHandler()
        {
            var inputText = View.LayerInput.text;
            
            int selectedLayer = 0;
            if (int.TryParse(inputText, out selectedLayer))
            {
                LevelEditorControler.CurrentLayerIndex = selectedLayer;
                LevelEditorControler.IndexChangedSignal.Dispatch();

            }
        }
        
        private void PathButtonHandler()
        {
            LevelEditorControler.AllLinesMode = !LevelEditorControler.AllLinesMode;
            View.PathModeButton.image.color =
                LevelEditorControler.AllLinesMode ? View.ToggleOnColor : View.ToggleOffColor;
        }
        
        private void ObstaclesVisible()
        {
            LevelEditorControler.VisibleObstacles = !LevelEditorControler.VisibleObstacles;
            View.ObstaclesVisible.image.color =
                LevelEditorControler.AllLinesMode ? View.ToggleOnColor : View.ToggleOffColor;
        }

        private void SetControlMode(EditorMode mode)
        {
            _editorMode = mode;
            ResetAll();
            switch (mode)
            {
                case EditorMode.None:
                    break;
                case EditorMode.Add:
                    View.AddButton.image.color = View.ToggleOnColor;
                    break;
                case EditorMode.Edit:
                    View.EditButton.image.color = View.ToggleOnColor;
                    break;
                case EditorMode.Save:
                    View.SaveButton.image.color = View.ToggleOnColor;
                    break;
            }
            
            EditModeChangedSignal.Dispatch(_editorMode);
            LevelEditorControler.EditorMode = mode;

        }

        private void ResetAll()
        {
            View.AddButton.image.color = View.ToggleOffColor;
            View.EditButton.image.color = View.ToggleOffColor;
            View.SaveButton.image.color = View.ToggleOffColor;
        }

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddButtonHandler();;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                EditButtonHandler();
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                SaveButtonHandler();
            }

            View.LevelSlider.value = LevelEditorControler.CurrentLayerIndex / 1000f;
            View.SliderVal.text = LevelEditorControler.CurrentLayerIndex.ToString();
        }
    }
}