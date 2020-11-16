using System;
using Framewerk.UI;
using Level;
using TMPro;
using UnityEngine;

namespace LevelEditor
{
    public class TargetDataMediator : ExtendedMediator
    {
        [Inject] public TargetDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }


        private int _targetIndex;
        private EditorMode _editorMode;

        public override void OnRegister()
        {
            base.OnRegister();
            //react to event
            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            EditModeChangedSignal.AddListener(EditModeChangedHandler);

            AddButtonListener(View.LayerUp, LayerUpHandler);
            AddButtonListener(View.LayerDown, LayerDownHandler);
            AddTmpInputListener(View.PositionInput, PositionInputHandler);
            AddTmpInputListener(View.DepthInput, DepthInputHandler);
            AddTmpDropdownListener(View.TypeDropdown, TypeDropdownHandler);

            AddButtonListener(View.RemoveButton, RemoveButtonHandler);
            
            
            var airtypes = Enum.GetValues(typeof(TargetType));
            View.TypeDropdown.options.Clear();
            foreach (var val in airtypes)
            {
                View.TypeDropdown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }
            
            
            SetEnabled(false);
        }
        
        public override void OnRemove()
        {
            EventObjectSelectedSignal.RemoveListener(EventObjectSelectedHandler);
            EditModeChangedSignal.RemoveListener(EditModeChangedHandler);
            
            base.OnRemove();
        }
        
        private void EditModeChangedHandler(EditorMode type)
        {
            _editorMode = type;
        }
        
        private void LayerUpHandler()
        {
            var target = EnemyEventsModel.GetTarget(_targetIndex);
            target.LayerIndex++;
            EnviromentMovedSignal.Dispatch();
            SetData(target);
            EnemyEventsModel.SaveChange();

        }
        
        private void LayerDownHandler()
        {
            var target = EnemyEventsModel.GetTarget(_targetIndex);
            target.LayerIndex--;
            EnviromentMovedSignal.Dispatch();
            SetData(target);
            EnemyEventsModel.SaveChange();

        }
        private void PositionInputHandler(string pos)
        {
            var target = EnemyEventsModel.GetTarget(_targetIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                target.StartPos = position;
                EnviromentMovedSignal.Dispatch();
            }
            SetData(target);
            EnemyEventsModel.SaveChange();
      
        }
        
        private void DepthInputHandler(string pos)
        {
            var target = EnemyEventsModel.GetTarget(_targetIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                target.Depth = position;
                EnviromentMovedSignal.Dispatch();
            }
            SetData(target);
            EnemyEventsModel.SaveChange();
      
        }
        
        private void TypeDropdownHandler(int type)
        {
            var target = EnemyEventsModel.GetTarget(_targetIndex);
            
            if (type != (int) target.TargetType)
            {
                target.TargetType = (TargetType)type;
                EnviromentMovedSignal.Dispatch();
                SetData(target);
                EnemyEventsModel.SaveChange();
            }
        }
        
        private void SetData(EditorTargetData objectData)
        {
            View.Index.text = objectData.LayerIndex.ToString();
            View.PositionInput.text = objectData.StartPos.ToString();
            View.DepthInput.text = objectData.Depth.ToString();
            View.TypeDropdown.value = (int)objectData.TargetType;
        }
        
        private void RemoveButtonHandler()
        {
            DeleteSelected();
        }
        
        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type != EnemyType.Target || _editorMode != EditorMode.Edit)
            {
                SetEnabled(false);
                return;
            }
            
             _targetIndex = index;
            var targetData = EnemyEventsModel.GetTarget(_targetIndex);
            if (targetData != null)
            {
                SetData(targetData);
                SetEnabled(true);
            }
            else
            {
                SetEnabled(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (_targetIndex != -1)
                {
                    DeleteSelected();
                }
            }
        }

        private void DeleteSelected()
        {
            EnemyEventsModel.RemoveTarget(_targetIndex);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }
        public void SetEnabled(bool isEnabled)
        {
            //disable
            View.gameObject.SetActive(isEnabled);
        } 
    }

}