using System.Collections.Generic;
using System.Linq;
using Common;
using Framewerk.Managers;
using Framewerk.UI;
using Level;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor
{
    public class MultiDataMediator : ExtendedMediator
    {
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public MultiDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public IUiManager UiManager { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }

        private EditorMode _editorMode;
        private float _obstacleLayerOffset;

        public override void OnRegister()
        {
            base.OnRegister();
            //react to event
            EditModeChangedSignal.AddListener(EditModeChangedHandler);
            LevelEditorControler.UpdateSelectionHandle.AddListener(EventObjectSelectedHandler);

            AddButtonListener(View.LayerUp, LayerUpHandler);
            AddButtonListener(View.LayerDown, LayerDownHandler);
            AddButtonListener(View.RemoveButton, RemoveButtonHandler);
            AddButtonListener(View.MoveLayerOffset, MoveLayerOffsetHandler);
            AddButtonListener(View.RecalculateOffsetX, ObstacleOffsetXHandler);
            AddButtonListener(View.RecalculateOffsetY, ObstacleOffsetYHandler);

            AddButtonListener(View.FlipXPos, FlipXPosHandler);
            AddButtonListener(View.FlipYPos, FlipYPosHandler);
            
            AddButtonListener(View.ExpandSpace, ExpandSpaceHandler);
            AddButtonListener(View.ShrinkSpace, ShrinkSpaceHandler);
            
            
            AddButtonListener(View.AddToLibraryButton, AddtoLibraryHandler);
            SetEnabled(false);
        }




        private void EditModeChangedHandler(EditorMode type)
        {
            _editorMode = type;
            bool multiEdit = LevelEditorControler.SelectedEvents.Count > 1;
            SetEnabled((type == EditorMode.Edit) && multiEdit);
            SetData();
        }

        private void EventObjectSelectedHandler()
        {
            if (_editorMode != EditorMode.Edit || LevelEditorControler.SelectedEvents.Count <= 1)
            {
                SetEnabled(false);
                return;
            }

            SetData();
            SetEnabled(true);
        }

        private void SetData()
        {
            View.SelectectCount.text = LevelEditorControler.SelectedEvents.Count.ToString();
        }

        public void SetEnabled(bool isEnabled)
        {
            //disable
            View.gameObject.SetActive(isEnabled);
        }

        private void LayerUpHandler()
        {
            foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
            {
                selectedEvent.LayerIndex++;
            }

            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void FlipXPosHandler()
        {
            foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
            {
                selectedEvent.StartPos = -selectedEvent.StartPos;
                IPathEditorData pathData = selectedEvent as IPathEditorData;
                if (pathData != null)
                {
                    pathData.MidPos = -pathData.MidPos;
                }
            }

            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void FlipYPosHandler()
        {
            int selectedEvents = LevelEditorControler.SelectedEvents.Count() - 1;
            int half = Mathf.FloorToInt(selectedEvents / 2f);
            
            
            for (int i = 0; i < LevelEditorControler.SelectedEvents.Count() / 2; i++)
            {
                float fisrtStart = LevelEditorControler.SelectedEvents[i].StartPos;
                float lastStart = LevelEditorControler.SelectedEvents[selectedEvents - i].StartPos;
                
                LevelEditorControler.SelectedEvents[i].StartPos = lastStart;
                LevelEditorControler.SelectedEvents[selectedEvents - i].StartPos = fisrtStart;
                
                IPathEditorData pathDataStart = LevelEditorControler.SelectedEvents[i] as IPathEditorData;
                IPathEditorData pathDatEnd = LevelEditorControler.SelectedEvents[selectedEvents - i] as IPathEditorData;
                if (pathDataStart != null && pathDatEnd != null)
                {
                    float fisrtMid = pathDataStart.MidPos;
                    float lastMid = pathDatEnd.MidPos;
                    
                    pathDataStart.MidPos = lastMid;
                    pathDatEnd.MidPos = fisrtMid;

                }
            }
     

            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }
   
        private void ExpandSpaceHandler()
        {
            var inputText = View.AddSpaceInput.text;
            int layerOffset = 0;
            int i = 0;


            List<IEditorData> selectedEvents = LevelEditorControler.SelectedEvents;
            // sort it just to be sure
            selectedEvents = selectedEvents.OrderBy(x => x.LayerIndex).ToList();
            if (int.TryParse(inputText, out layerOffset))
            {
                int offset = 0;
                int curLayerIndex = 0;
                foreach (var selectedEvent in selectedEvents)
                {
                    selectedEvent.LayerIndex += i;
                    
                    if (curLayerIndex != selectedEvent.LayerIndex)
                    {
                        i++;
                        curLayerIndex  = selectedEvent.LayerIndex;
                    }
                }
            }
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void ShrinkSpaceHandler()
        {
            var inputText = View.AddSpaceInput.text;
            int layerOffset = 0;
            int i = 0;

            List<IEditorData> selectedEvents = LevelEditorControler.SelectedEvents;
            // sort it just to be sure
            selectedEvents = selectedEvents.OrderBy(x => x.LayerIndex).ToList();
            if (int.TryParse(inputText, out layerOffset))
            {
                int offset = 0;
                int curLayerIndex = 0;
                foreach (var selectedEvent in selectedEvents)
                {
                    selectedEvent.LayerIndex -= i;
                    
                    if (curLayerIndex != selectedEvent.LayerIndex)
                    {
                        i++;
                        curLayerIndex  = selectedEvent.LayerIndex;
                    }
                }
            }
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void LayerDownHandler()
        {
            foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
            {
                selectedEvent.LayerIndex--;
            }

            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }


        private void MoveLayerOffsetHandler()
        {
            var inputText = View.LayerOffset.text;
            int layerOffset = 0;
            if (int.TryParse(inputText, out layerOffset))
            {
                foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
                {
                    selectedEvent.LayerIndex += layerOffset;
                }

                EnviromentMovedSignal.Dispatch();
                EnemyEventsModel.SaveChange();
            }
        }

        private void ObstacleOffsetXHandler()
        {
            var inputText = View.ObstaclesOffsetX.text;
            float layerOffset = 0;
            int i = 0;
            if (float.TryParse(inputText, out layerOffset))
            {
                float offset = 0f;
                foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
                {
                    offset = i * layerOffset;
                    selectedEvent.StartPos = offset;
                    IPathEditorData pathData = selectedEvent as IPathEditorData;
                    if (pathData != null)
                    {
                        pathData.MidPos = offset;
                    }
                    i++;
                }

                EnviromentMovedSignal.Dispatch();
                EnemyEventsModel.SaveChange();
                
            }
        }
        
        private void ObstacleOffsetYHandler()
        {
            var inputText = View.ObstaclesOffsetY.text;
            float layerOffset = 0;
            int i = 0;
            if (float.TryParse(inputText, out layerOffset))
            {
                float offset = 0f;
                foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
                {
                    offset = i * layerOffset;
                    //selectedEvent.StartHeight = offset;
                    IPathEditorData pathData = selectedEvent as IPathEditorData;
                    if (pathData != null)
                    {
                        pathData.MidHeight = offset;
                    }
                    i++;
                }

                EnviromentMovedSignal.Dispatch();
                EnemyEventsModel.SaveChange();
                
            }
        }
        
        private void AddtoLibraryHandler()
        {
            // show add to library popup
            UiManager.InstantiateView<AddtoLibraryPopupView>(ResourcePath.LEVEL_EDITOR);
        }

        private void RemoveButtonHandler()
        {
            foreach (var selectedEvent in LevelEditorControler.SelectedEvents)
            {
                EnemyEventsModel.RemoveEventData(selectedEvent);
            }

            LevelEditorControler.SelectedEvents.Clear();
            LevelEditorControler.UpdateSelected();

            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }
    }
}