using System;
using Framewerk.UI;
using Level;
using TMPro;
using UnityEngine;

namespace LevelEditor
{
    
    public class FishDataMediator : ExtendedMediator
    {
        [Inject] public FishDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }


        private int _fishIndex;
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
            AddTmpDropdownListener(View.TypeDropdown, TypeDropdownHandler);

            AddButtonListener(View.RemoveButton, RemoveButtonHandler);
            
            
            var airtypes = Enum.GetValues(typeof(BombType));
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
            var fish = EnemyEventsModel.GetFish(_fishIndex);
            fish.LayerIndex++;
            EnviromentMovedSignal.Dispatch();
            SetData(fish);
            EnemyEventsModel.SaveChange();

        }
        
        private void LayerDownHandler()
        {
            var fish = EnemyEventsModel.GetFish(_fishIndex);
            fish.LayerIndex--;
            EnviromentMovedSignal.Dispatch();
            SetData(fish);
            EnemyEventsModel.SaveChange();

        }
        private void PositionInputHandler(string pos)
        {
            var fish = EnemyEventsModel.GetFish(_fishIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                fish.StartPos = position;
                EnviromentMovedSignal.Dispatch();
            }
            SetData(fish);
            EnemyEventsModel.SaveChange();
      
        }
        
        private void TypeDropdownHandler(int type)
        {
            var fish = EnemyEventsModel.GetFish(_fishIndex);
            
            if (type != (int) fish.FishType)
            {
                fish.FishType = (BombType)type;
                EnviromentMovedSignal.Dispatch();
                SetData(fish);
                EnemyEventsModel.SaveChange();
            }
        }
        
        private void SetData(EditorFishData objectData)
        {
            View.Index.text = objectData.LayerIndex.ToString();
            View.PositionInput.text = objectData.StartPos.ToString();
            View.TypeDropdown.value = (int)objectData.FishType;
        }
        
        private void RemoveButtonHandler()
        {
            DeleteSelected();
        }
        
        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type != EnemyType.Fish || _editorMode != EditorMode.Edit)
            {
                SetEnabled(false);
                return;
            }
            
             _fishIndex = index;
            var fishData = EnemyEventsModel.GetFish(_fishIndex);
            if (fishData != null)
            {
                SetData(fishData);
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
                if (_fishIndex != -1)
                {
                    DeleteSelected();
                }
            }
        }

        private void DeleteSelected()
        {
            EnemyEventsModel.RemoveFish(_fishIndex);
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